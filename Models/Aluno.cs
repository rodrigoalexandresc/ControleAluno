using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleAluno.Models
{
    public class Aluno
    {
        public Aluno()
        {
            Notas = new List<Nota>();
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Nota> Notas { get; set; }
    }
}
