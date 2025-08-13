using ClinicSystem.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClinicSystem.DAL.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly MyContext _context;

    public GenericRepository(MyContext context)
    {
        this._context = context;
    }
    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public Task Add(T entity)
    {
       _context.Set<T>().Add(entity);
        return Task.CompletedTask;
    }
    public Task Update(T entity)
    {
         _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
         _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
    public async Task<bool> Any(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AnyAsync(predicate);
    }

}


