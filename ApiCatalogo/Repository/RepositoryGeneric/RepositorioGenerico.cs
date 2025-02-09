using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiCatalogo.Context;

namespace ApiCatalogo.Repository.RepositoryGeneric
{
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {

        protected readonly AppDbContext _context;

        public RepositorioGenerico(AppDbContext context)
        {
            _context = context;

        }


        public IEnumerable<T> BuscarTodos()
        {
            return _context.Set<T>().ToList();
        }
        public T? Buscar(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Criar(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public T Atualizar(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public T Deletar(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}