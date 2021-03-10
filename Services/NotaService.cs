using ControleAluno.Models;
using ControleAluno.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleAluno.Services
{
    public class NotaService
    {
        private readonly ControleAlunoDbContext context;

        public NotaService(ControleAlunoDbContext context)
        {
            this.context = context;
        }

        public void CompilarDados()
        {
            ConfigurarAmbienteBancoDados();

            var disciplinas = CriarDisciplinasPadrao();
            var alunos = CriarAlunos();            
            GerarNotas(alunos, disciplinas);

            disciplinas.ForEach(d => context.Disciplina.Add(d));
            alunos.ForEach(a => context.Aluno.Add(a));

            context.SaveChanges();
        }

        private void ConfigurarAmbienteBancoDados()
        {
            var conexao = context.Database.GetDbConnection();

            conexao.Execute("DROP TABLE IF EXISTS NOTA;");
            conexao.Execute("DROP TABLE IF EXISTS ALUNO;");
            conexao.Execute("DROP TABLE IF EXISTS DISCIPLINA;");

            conexao.Execute("CREATE TABLE ALUNO ( ID INT IDENTITY, NOME VARCHAR(255), PRIMARY KEY (Id) )");
            conexao.Execute("CREATE TABLE DISCIPLINA ( ID INT, TITULO VARCHAR(100), PRIMARY KEY (Id) )");
            conexao.Execute(@"
                CREATE TABLE NOTA ( 
                    ID INT IDENTITY, 
                    AlunoID INT NOT NULL,
                    DisciplinaId INT NOT NULL,
                    Valor NUMERIC(4,2),
                    PRIMARY KEY (Id),
                    CONSTRAINT nota_aluno_fkey FOREIGN KEY (AlunoId) REFERENCES ALUNO ON UPDATE CASCADE ON DELETE CASCADE,
                    CONSTRAINT nota_disciplina_fkey FOREIGN KEY (DisciplinaId) REFERENCES DISCIPLINA ON UPDATE CASCADE ON DELETE CASCADE
                )");
        }

        private void GerarNotas(IList<Aluno> alunos, IList<Disciplina> disciplinas)
        {
            var random = new Random();
            alunos.ToList().ForEach(aluno =>
            {
                disciplinas.ToList().ForEach(disciplina => aluno.Notas.Add(new Nota
                {
                    DisciplinaId = disciplina.Id,
                    Valor = random.Next(50, 100) / 10
                }));
            });
        }

        private List<Aluno> CriarAlunos()
        {
            var primeirosNomes = new List<string> { "Rodrigo", "Patricia", "Maria", "Ana", "Fernando", "Fernanda", "Cristina", "Pedro", "Joarez", "Celia" };
            var nomesDoMeio = new List<string> { "Silvestrin", "Sousa", "Gil", "Porto", "Costa", "Afonso", "Telmo", "Senra", "Benitez", "Jansen" };
            var ultimosNomes = new List<string> { "Silva", "Cruz", "Leitão", "Sá", "Ceschi", "Lima", "Letterman", "Franscechini", "Bandini", "Harari" };

            var alunos = primeirosNomes
                .Join(nomesDoMeio, o => true, i => true, (p, m) => $"{p} {m}")
                .Join(ultimosNomes, o => true, i => true, (nome, ultimo) => $"{nome} {ultimo}")
                .Select(nome => new Aluno { Nome = nome })
                .ToList();

            return alunos;
        }

        private List<Disciplina> CriarDisciplinasPadrao()
        {
            var disciplinasPadrao = new List<Disciplina>
            {
                new Disciplina { Id = 1, Titulo = "Matemática" },
                new Disciplina { Id = 2, Titulo = "Português" },
                new Disciplina { Id = 3, Titulo = "História" },
                new Disciplina { Id = 4, Titulo = "Geografica" },
                new Disciplina { Id = 5, Titulo = "Inglês" },
                new Disciplina { Id = 6, Titulo = "Biologia" },
                new Disciplina { Id = 7, Titulo = "FIlosofia" },
                new Disciplina { Id = 8, Titulo = "Física" },
                new Disciplina { Id = 9, Titulo = "Química" }
            };

            return disciplinasPadrao;
        }
    }
}
