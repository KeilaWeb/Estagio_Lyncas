using Azure;
using Dominio.Models.ApplicationUser;
using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;
using Dominio.Models.ViewModelVendedor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository.TokenRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<TokenService> _logger;
        private readonly IConfiguration _configuration; //usado para acessar e manipular valores de configuração definidos
                                                        //em diferentes fontes, como arquivos de configuração JSON

        public TokenService(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository, ILogger<TokenService> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _logger = logger;   
            _configuration = configuration;
        }
        public async Task<object> Login(LoginModel model)
        {
            try
            {
                // Procura o usuário pelo e-mail fornecido no modelo
                var user = await _tokenRepository.TokenLogin(model.Email!);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Senha!))
                {
                    // Obtém os papéis (roles) do usuário
                    var userRoles = await _userManager.GetRolesAsync(user);
                    // Cria uma lista de reivindicações (claims) de autenticação
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim("id", user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    // Para cada papel (role) do usuário, adiciona uma reivindicação de papel (role) à lista de reivindicações
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    // Gera um token de acesso com base nas reivindicações de autenticação e na configuração
                    var token = GenerateAccessToken(authClaims, _configuration);
                    // Gera um token de atualização (refresh token)
                    var refreshToken = GenerateRefreshToken();
                    // Tenta converter o tempo de validade do refresh token para um valor inteiro
                    if (int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes))
                    {
                        // Define o refresh token e o tempo de expiração do refresh token para o usuário
                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                        // Atualiza as informações do usuário no armazenamento
                        await _userManager.UpdateAsync(user);
                    }
                    // Retorna um resultado de objeto contendo o token de acesso, o refresh token e o tempo de expiração do token de acesso
                    return new ObjectResult(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo
                    });
                }
                else
                {
                    return new UnauthorizedResult();
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    Error = $"Ocorreu um erro durante o login: {ex.Message}"
                };
            }
        }

        public async Task<Resposta> CadastrarUsuario(RegistroModel model)
        {
            try
            {
                var usuarioJaExiste = await _tokenRepository.UsuarioExistente(model.Email);
                if (usuarioJaExiste)
                {
                    return new Resposta { Status = "Error", Message = "Usuário já existe" };
                }

                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.NomeUsuario
                };


                var resultadoNovoUsuario = await _tokenRepository.CadastrarUsuario(user, model.Senha!);

                if (!resultadoNovoUsuario)
                {
                   return new Resposta{ Status = "Error", Message = "Falha ao criar usuário" };
                }
                return new Resposta { Status = "Sucesso", Message = "Usuário criado com sucesso" };

            }
            catch (Exception ex)
            {
                return new Resposta { Status = "Error", Message = $"Erro durante o cadastro: {ex.Message}" };
            }
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

        public async Task<Resposta> AddUserToRole(string email, string roleName)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, $"User {user.Email} adicionado para {roleName} role");
                        return new Resposta { Status = "Success", Message = $"Usuário {user.Email} adicionado para a função {roleName}" };
                    }
                    else
                    {
                        _logger.LogInformation(1, $"Erro: não é possível adicionar usuário {user.Email} para a função {roleName}");
                        return new Resposta { Status = "Error", Message = $"Erro: não é possível adicionar usuário {user.Email} para a função {roleName}" };
                    }
                }
                else
                {
                    return new Resposta { Status = "Error", Message = "Não foi possível encontrar o usuário" };
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Status = "Error", Message = $"Ocorreu um erro durante a adesão da role: {ex.Message}" };
            }
        }



        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
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

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(secureRandomBytes);
            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
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
            if(securityToken is not JwtSecurityToken jwtSecurityToken || 
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
