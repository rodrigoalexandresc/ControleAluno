using ControleAluno.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControleAluno.Repositories
{
    public class UsuarioRepository
    {
        private readonly ControleAlunoDbContext context;

        public UsuarioRepository(ControleAlunoDbContext context)
        {
            this.context = context;
        }

        public Usuario ObterUsuarioViaLogin(LoginViewModel loginViewModel)
        {
            return context.Usuario.FirstOrDefault(u => u.Login == loginViewModel.Usuario && u.Senha == loginViewModel.Senha);
        }
    }
}
