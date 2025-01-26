using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiCatalogo.Repository.RepositoryGeneric
{
    public interface IRepositorioGenerico <T>
    {
        IEnumerable<T>BuscarTodos();
        T? Buscar(Expression<Func<T, bool>> predicate);
        T Criar(T entity);
        T Atualizar(T entity);
        T Deletar(T entity);
        
    }
}