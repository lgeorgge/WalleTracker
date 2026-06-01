using Microsoft.EntityFrameworkCore;
using WalletTracker.Application.Interfaces;
using WalletTracker.Application.Specifications;
using WalletTracker.Domain.Common;
using WalletTracker.Infrastructure.Data;
using WalletTracker.Infrastructure.Specifications;

namespace WalletTracker.Infrastructure.Repositories;

public class Repository<T> : IRepository<T>
    where T : BaseEntity
{
    protected readonly WalletDBContext _walletDBContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(WalletDBContext walletDBContext)
    {
        _walletDBContext = walletDBContext;
        _dbSet = _walletDBContext.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T>? GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public Task<T?> FindFirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    )
    {
        var evaluatedQuery = SpecEvaluator.GetQuery(_dbSet, specification);
        return evaluatedQuery.FirstOrDefaultAsync();
    }
}
