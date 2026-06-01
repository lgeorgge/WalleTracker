using WalletTracker.Application.Specifications;
using WalletTracker.Domain.Common;

namespace WalletTracker.Application.Interfaces;

public interface IRepository<T>
    where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);

    Task<T?> FindFirstOrDefaultAsync(ISpecification<T> specification);

    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
