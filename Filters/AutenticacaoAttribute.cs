using ControleAluno.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ControleAluno.Filters
{
    public class AutenticacaoAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var usuarioLogado = context.HttpContext.Session.GetString(LoginConsts.UsuarioLogado);
            if (string.IsNullOrEmpty(usuarioLogado))
                context.Result = new RedirectToActionResult("Index", "Login", new { });
        }
    }
}
