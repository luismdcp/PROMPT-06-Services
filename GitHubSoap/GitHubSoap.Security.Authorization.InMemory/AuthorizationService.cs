using GitHubSoap.Security.Contracts;

namespace GitHubSoap.Security.Authorization.InMemory
{
    public class AuthorizationService : IAuthorizationService
    {
        public bool Authorize(string user, string operation)
        {
            return true;
        }
    }
}