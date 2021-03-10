using ClosedXML.Excel;
using ControleAluno.Models;
using ControleAluno.Repositories;
using System.Linq;

namespace ControleAluno.Services
{
    public class RelatorioService
    {
        private readonly ControleAlunoDbContext context;

        public RelatorioService(ControleAlunoDbContext controleAlunoDbContext)
        {
            this.context = controleAlunoDbContext;
        }

        public void GerarRelatorio()
        {
            RelatorioDTO relatorioDTO = ObterDados();
            GravarArquivoExcel(relatorioDTO);
        }

        private void GravarArquivoExcel(RelatorioDTO relatorioDTO)
        {
            using (var workbook = new XLWorkbook())
            {
                CriarPlanilhaAlunos(relatorioDTO, workbook);
                CriarPlanilhaNotas(relatorioDTO, workbook);
                CriarPlanilhaMedias(relatorioDTO, workbook);

                workbook.SaveAs("TempData/Relatorio.xlsx");
            }
        }

        private RelatorioDTO ObterDados()
        {
            var relatorioDTO = new RelatorioDTO();

            relatorioDTO.Alunos = this.context.Aluno
                .Select(a => new RelatorioAlunoDTO { Nome = a.Nome })
                .OrderBy(a => a.Nome)
                .ToList();

            relatorioDTO.NotasPorAluno = this.context.Aluno
                .SelectMany(a => a.Notas.Select(n => new RelatorioAlunoNotaDTO { Aluno = a.Nome, Disciplina = n.Disciplina.Titulo, Nota = n.Valor }))
                .OrderBy(n => n.Aluno).ThenBy(n => n.Disciplina)
                .ToList();
            relatorioDTO.MediasPorDisciplina = this.context.Nota
                .GroupBy(g => g.Disciplina.Titulo)                
                .Select(g => new RelatorioMediaDisciplinaDTO { Disciplina = g.Key, NotaMedia = g.Average(o => o.Valor) })
                .OrderBy(m => m.Disciplina)
                .ToList();

            return relatorioDTO;
        }

        private void CriarPlanilhaMedias(RelatorioDTO relatorioDTO, XLWorkbook workbook)
        {
            var mediasSheet = workbook.AddWorksheet("Médias");
            mediasSheet.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;
            mediasSheet.Cell(1, 1).SetValue("Disciplina");
            mediasSheet.Cell(1, 2).SetValue("Nota Média");

            for (int i = 0; i < relatorioDTO.MediasPorDisciplina.Count; i++)
            {
                var row = i + 2;
                mediasSheet.Cell(row, 1).SetValue(relatorioDTO.MediasPorDisciplina[i].Disciplina);
                mediasSheet.Cell(row, 2).SetValue(relatorioDTO.MediasPorDisciplina[i].NotaMedia);
            }
            mediasSheet.Columns().AdjustToContents();
        }

        private void CriarPlanilhaNotas(RelatorioDTO relatorioDTO, XLWorkbook workbook)
        {
            var notasSheet = workbook.AddWorksheet("Notas");
            notasSheet.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
            notasSheet.Cell(1, 1).Value = "Aluno";
            notasSheet.Cell(1, 2).Value = "Disciplina";
            notasSheet.Cell(1, 3).Value = "Nota";
            for (int i = 0; i < relatorioDTO.NotasPorAluno.Count; i++)
            {
                var row = i + 2;
                notasSheet.Cell(row, 1).Value = relatorioDTO.NotasPorAluno[i].Aluno;
                notasSheet.Cell(row, 2).Value = relatorioDTO.NotasPorAluno[i].Disciplina;
                notasSheet.Cell(row, 3).Value = relatorioDTO.NotasPorAluno[i].Nota;
            }
            notasSheet.Columns().AdjustToContents();
        }

        private void CriarPlanilhaAlunos(RelatorioDTO relatorioDTO, XLWorkbook workbook)
        {
            var alunosSheet = workbook.AddWorksheet("Alunos");
            alunosSheet.Row(1).Style.Fill.BackgroundColor = XLColor.LightGray;
            alunosSheet.Cell(1, 1).SetValue("Nome");
            for (int i = 0; i < relatorioDTO.Alunos.Count; i++)
            {
                var row = i + 2;
                alunosSheet.Cell(row, 1).SetValue(relatorioDTO.Alunos[i].Nome);
            }
            alunosSheet.Columns().AdjustToContents();
        }
    }
}
