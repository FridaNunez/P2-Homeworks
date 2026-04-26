using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZenCare.Application.Interfaces;
using ZenCare.Infrastructure.Data;

namespace ZenCare.Infrastructure.Repositories;

/// <summary>
/// Generic repository - implements basic CRUD operations.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ZenCareDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ZenCareDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public virtual void Update(T entity) =>
        _dbSet.Update(entity);

    public virtual void Delete(T entity) =>
        _dbSet.Remove(entity);

    public virtual async Task<bool> ExistsAsync(Guid id) =>
        await _dbSet.FindAsync(id) != null;
}