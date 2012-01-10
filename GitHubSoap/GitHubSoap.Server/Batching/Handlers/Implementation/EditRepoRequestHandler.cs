using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class EditRepoRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (EditRepoRequest) request;
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            var repoResult = reposService.Edit(typedRequest.User, typedRequest.Password, typedRequest.Repo, typedRequest.EditRepo);

            return new RepoResponse(repoResult);
        }
    }
}