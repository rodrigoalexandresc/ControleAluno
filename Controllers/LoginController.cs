using ControleAluno.Models;
using ControleAluno.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleAluno.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginService loginService;

        public LoginController(LoginService loginService)
        {
            this.loginService = loginService;
        }

        public IActionResult Index()
        {
            loginService.CriarDadosBasicosLogin();           
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginViewModel viewModel)
        {            
            if (loginService.EfetuarLogin(viewModel))
            {
                return RedirectToAction("Index", "Relatorio");
            }
                

            return View("AcessoNegado");
        }
    }
}
