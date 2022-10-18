using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IUserRepository
    {
       Task<User>   AuthenticateAsync(string UserName, string password);
    }
}
