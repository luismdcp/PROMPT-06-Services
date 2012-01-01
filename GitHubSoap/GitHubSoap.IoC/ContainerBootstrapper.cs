using GitHubSoap.Repositories.Contracts;
using GitHubSoap.Repositories.Implementation;
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
                x.ForRequestedType<IIssuesRepository>().TheDefaultIsConcreteType<IssuesRepository>();
                x.ForRequestedType<IReposRepository>().TheDefaultIsConcreteType<ReposRepository>();
                x.ForRequestedType<IIssuesService>().TheDefaultIsConcreteType<IssuesService>();
                x.ForRequestedType<IReposService>().TheDefaultIsConcreteType<ReposService>();
            });
        }
    }
}