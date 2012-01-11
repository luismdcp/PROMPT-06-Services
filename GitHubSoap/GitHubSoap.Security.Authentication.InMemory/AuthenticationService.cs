using GitHubSoap.Security.Contracts;

namespace GitHubSoap.Security.Authentication.InMemory
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool Authenticate(string user, string password)
        {
            return true;
        }
    }
}