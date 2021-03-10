using ControleAluno.Models;
using ControleAluno.Repositories;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ControleAluno.Services
{
    public class LoginService
    {
        private readonly ControleAlunoDbContext context;
        private readonly UsuarioRepository usuarioRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginService(ControleAlunoDbContext context, UsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.usuarioRepository = usuarioRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void CriarDadosBasicosLogin()
        {
            ConfigurarAmbienteBancoDados();
            AdicionarDadosLogin();
        }

        public bool EfetuarLogin(LoginViewModel viewModel)
        {
            var usuario = usuarioRepository.ObterUsuarioViaLogin(viewModel);
            if (usuario != null)
            {
                httpContextAccessor.HttpContext.Session.SetString(LoginConsts.UsuarioLogado, usuario.Login);
                return true;
            }
            return false;
        }

        private void ConfigurarAmbienteBancoDados()
        {
            var conexao = context.Database.GetDbConnection();

            conexao.Execute("DROP TABLE IF EXISTS USUARIO;");

            conexao.Execute("CREATE TABLE USUARIO ( ID INT IDENTITY, LOGIN VARCHAR(255), SENHA VARCHAR(255), PRIMARY KEY (Id) )");
        }

        private void AdicionarDadosLogin()
        {
            context.Usuario.Add(new Models.Usuario
            {
                Login = "candidato-evolucional",
                Senha = "123456"
            });
            context.SaveChanges();
        }
    }
}
