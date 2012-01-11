
namespace GitHubSoap.Security.Contracts
{
    public interface IAuthenticationService
    {
        bool Authenticate(string user, string password);
    }
}