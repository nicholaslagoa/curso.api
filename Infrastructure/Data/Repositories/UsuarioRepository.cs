using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CursoDbContext _contexto;
        public UsuarioRepository(CursoDbContext contexto)
        {
            _contexto = contexto;
        }
        public void Adicionar(Usuario usuario)
        {
            _contexto.Usuario.Add(usuario);
        }
        public void Commit()
        {
            _contexto.SaveChanges();
        }
        public Usuario ObterUsuario(string login)
        {
            return _contexto.Usuario.FirstOrDefault(u => u.Login == login);
        }
    }
}
