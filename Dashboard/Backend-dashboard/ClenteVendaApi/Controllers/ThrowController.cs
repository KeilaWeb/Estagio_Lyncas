using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClienteVendaApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ThrowController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError() =>
            Problem();

        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment(
            [FromServices] IHostEnvironment hostEnvironment)
        {
            if(!hostEnvironment.IsDevelopment())
            {
                return NotFound();
            }

            //verifica qual foi o problema
            var exceptionHandlerFeature =
                HttpContext.Features.Get<IExceptionHandlerFeature>();

            // retorna com o detalhe, o titulo e a mensagem do problema
            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }
    }
}
