using System.Linq.Expressions;

namespace WalletTracker.Application.Specifications;

public interface ISpecification<T>
{
    // Criteria
    List<Expression<Func<T, bool>>>? Criteria { get; }

    // OrderBy
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDesending { get; }

    // Pagination
    public int Skip { get; }
    public int Take { get; }
    public bool IsPaginationEnabled { get; }
}
