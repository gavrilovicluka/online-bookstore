using System.Linq.Expressions;
using Bookstore.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Persistence.Repositories;

public abstract class RepositoryBase<TContext, T> : IRepositoryBase<T> where T : class where TContext : DbContext
{
    protected TContext _context;

    public RepositoryBase(TContext context)
    {
        _context = context;
    }
    
    public IQueryable<T> FindAll()
    {
        return _context.Set<T>();
    }

    public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}