using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class CreateRepoRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (CreateRepoRequest) request;
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            var repoResult = reposService.Create(typedRequest.User, typedRequest.Password, typedRequest.CreateRepo);

            return new RepoResponse(repoResult);
        }
    }
}