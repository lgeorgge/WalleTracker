using WalletTracker.Application.Specifications;
using WalletTracker.Domain.Common;

namespace WalletTracker.Application.Interfaces;

public interface IRepository<T>
    where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<T?> FindFirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Remove(T entity);
}
