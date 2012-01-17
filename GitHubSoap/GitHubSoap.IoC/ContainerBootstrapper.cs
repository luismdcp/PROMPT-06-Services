using GitHubSoap.Repositories.Contracts;
using GitHubSoap.Repositories.Implementation;
using GitHubSoap.Security.Authentication.InMemory;
using GitHubSoap.Security.Authorization.InMemory;
using GitHubSoap.Security.Contracts;
using GitHubSoap.Services.Contracts;
using GitHubSoap.Services.Implementation;
using StructureMap;

namespace GitHubSoap.IoC
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.For<IIssuesRepository>().Use<IssuesRepository>();
                x.For<IReposRepository>().Use<ReposRepository>();
                x.For<IIssuesService>().Use<IssuesService>();
                x.For<IReposService>().Use<ReposService>();
                x.For<IAuthenticationService>().Use<AuthenticationService>();
                x.For<IAuthorizationService>().Use<AuthorizationService>();
            });
        }
    }
}