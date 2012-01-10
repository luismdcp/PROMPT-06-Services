using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class GetRepoRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (GetRepoRequest) request;
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            var repoResult = reposService.Get(typedRequest.User, typedRequest.Repo);

            return new RepoResponse(repoResult);
        }
    }
}