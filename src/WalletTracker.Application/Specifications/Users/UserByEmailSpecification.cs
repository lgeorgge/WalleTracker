using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Specifications.Users;

public class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
    {
        Criteria = U => U.Email == email;
    }
}
