using System.Collections.Generic;

namespace ControleAluno.Models
{
    public class RelatorioDTO
    {
        public IList<RelatorioAlunoDTO> Alunos { get; set; }

        public IList<RelatorioAlunoNotaDTO> NotasPorAluno { get; set; }

        public IList<RelatorioMediaDisciplinaDTO> MediasPorDisciplina { get; set; }
    }

    public class RelatorioAlunoDTO
    {
        public string Nome { get; set; }
    }

    public class RelatorioAlunoNotaDTO
    {
        public string Aluno { get; set; }
        public string Disciplina { get; set; }
        public decimal Nota { get; set; }
    }

    public class RelatorioMediaDisciplinaDTO
    {
        public string Disciplina { get; set; }
        public decimal NotaMedia { get; set; }
    }
}
