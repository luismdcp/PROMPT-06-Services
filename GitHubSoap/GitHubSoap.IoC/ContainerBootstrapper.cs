using GitHubSoap.Repositories.Contracts;
using GitHubSoap.Repositories.REST;
using GitHubSoap.Security.Authentication.InMemory;
using GitHubSoap.Security.Authorization.InMemory;
using GitHubSoap.Security.Contracts;
using GitHubSoap.Services.Contracts;
using GitHubSoap.Services.Persistence;
using StructureMap;

namespace GitHubSoap.IoC
{
    public static class ContainerBootstrapper
    {
        public static void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.ForRequestedType<IIssuesRepository>().TheDefaultIsConcreteType<IssuesRepository>();
                x.ForRequestedType<IReposRepository>().TheDefaultIsConcreteType<ReposRepository>();
                x.ForRequestedType<IIssuesService>().TheDefaultIsConcreteType<IssuesService>();
                x.ForRequestedType<IReposService>().TheDefaultIsConcreteType<ReposService>();
                x.ForRequestedType<IAuthenticationService>().TheDefaultIsConcreteType<AuthenticationService>();
                x.ForRequestedType<IAuthorizationService>().TheDefaultIsConcreteType<AuthorizationService>();
            });
        }
    }
}