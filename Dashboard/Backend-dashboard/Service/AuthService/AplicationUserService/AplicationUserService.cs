using Dominio.Models.ApplicationUser;
using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.TokenRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service.AuthService.AuthService
{
    public class AplicationUserService : IAplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthRepository _tokenRepository;
        private readonly IConfiguration _configuration; //usado para acessar e manipular valores de configuração definidos
                                                        //em diferentes fontes, como arquivos de configuração JSON

        public AplicationUserService(UserManager<ApplicationUser> userManager, IAuthRepository tokenRepository, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }
        public async Task<object> Login(LoginModel model)
        {
            try
            {
                var user = await _tokenRepository.Login(model.Email!, model.Senha!);
                if (user != null)
                {
                    return await GenerateTokens(user);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro durante o login: {ex.Message}");
            }
        }

        private async Task<object> GenerateTokens(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("id", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateAccessToken(authClaims, _configuration);
            var refreshToken = GenerateRefreshToken();

            if (int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes))
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                await _userManager.UpdateAsync(user);
            }
            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
            return new
            {
                Token = tokenResult,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }

        public async Task<Resposta> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return new Resposta { Status = "Error", Message = "Solicitação de cliente inválida" };
            }
            string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenModel));
            var principal = GetPrincipalFromExpiredToken(accessToken!, _configuration);
            if (principal == null)
            {
                return new Resposta { Status = "Error", Message = "Token de acesso/token de atualização inválido" };
            }
            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new Resposta { Status = "Error", Message = "Token de acesso/token de atualização inválido" };
            }
            var newAccessToken = GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return new Resposta
            {
                Status = "Success",
                Message = "Tokens atualizados com sucesso",
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        public async Task<Resposta> RevokeToken(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return new Resposta { Status = "Error", Message = "Nome de usuário inválido" };
            }
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return new Resposta { Status = "Success", Message = "Token revogado com sucesso" };
        }

        public static JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ??
                      throw new InvalidOperationException("Chave secreta invalida");

            var privatekey = Encoding.UTF8.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privatekey),
                                     SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT")
                                                    .GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        public static string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);
            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var Secretkey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Chave invalida");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secretkey)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token,
                            tokenValidationParameters,
                            out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                            !jwtSecurityToken.Header.Alg
                            .Equals(SecurityAlgorithms.HmacSha256,
                            StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token Invalido");
            }
            return principal;
        }

    }
}
