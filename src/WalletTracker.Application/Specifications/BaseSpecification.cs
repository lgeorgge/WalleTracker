using System.Linq.Expressions;

namespace WalletTracker.Application.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
    where T : class
{
    public Expression<Func<T, bool>>? Criteria { get; protected set; }

    public Expression<Func<T, object>>? OrderBy { get; protected set; }

    public Expression<Func<T, object>>? OrderByDesending { get; protected set; }

    public int Skip { get; protected set; }
    public int Take { get; protected set; }

    public bool IsPaginationEnabled { get; protected set; }

    protected void ApplyPagination(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPaginationEnabled = true;
    }
}
