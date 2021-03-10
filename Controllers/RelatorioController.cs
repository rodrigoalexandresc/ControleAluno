using ControleAluno.Models;
using ControleAluno.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ControleAluno.Filters;

namespace ControleAluno.Controllers
{

    [AutenticacaoAttribute]
    public class RelatorioController : Controller
    {
        private readonly NotaService notaService;
        private readonly RelatorioService relatorioService;

        public RelatorioController(NotaService notaService, RelatorioService relatorioService)
        {
            this.notaService = notaService;
            this.relatorioService = relatorioService;
        }

        public IActionResult Index()
        {
            return View(new RelatorioViewModel());
        }

        public IActionResult CompilarDados()
        {
            notaService.CompilarDados();
            return View("Index", new RelatorioViewModel { DadosParaRelatorioCompilados = true });
        }

        public IActionResult GerarRelatorio()
        {
            relatorioService.GerarRelatorio();

            return RetornarArquivoGravado();
        }

        private IActionResult RetornarArquivoGravado()
        {
            string filename = "Relatorio.xlsx";
            string filepath = "TempData/Relatorio.xlsx";
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var cd = new ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }
    }
}
