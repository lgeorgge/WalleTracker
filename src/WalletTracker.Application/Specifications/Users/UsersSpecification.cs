using WalletTracker.Application.Features.Users;
using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Specifications.Users;

public class UsersSpecification : BaseSpecification<User>
{
    public UsersSpecification(UserQueryParameters userQueryParameters)
    {
        ApplyPagination(
            (userQueryParameters.Page - 1) * userQueryParameters.PageSize,
            userQueryParameters.PageSize
        );
        if (!string.IsNullOrWhiteSpace(userQueryParameters.Search))
        {
            string keyword = userQueryParameters.Search;
            AddCriteria(U =>
                U.FirstName.Contains(keyword)
                || U.LastName.Contains(keyword)
                || U.Email.Contains(keyword)
            );
        }
    }
}
