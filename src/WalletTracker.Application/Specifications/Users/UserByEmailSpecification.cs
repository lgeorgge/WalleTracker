using WalletTracker.Domain.Entities;

namespace WalletTracker.Application.Specifications.Users;

public class UserByEmailSpecification : BaseSpecification<User>
{
    public UserByEmailSpecification(string email)
    {
        AddCriteria(U => U.Email.ToLower() == email.ToLower());
    }
}
