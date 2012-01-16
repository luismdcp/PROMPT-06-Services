
namespace GitHubSoap.Security.Contracts
{
    public interface IAuthorizationService
    {
        bool Authorize(string user, string operation);
    }
}