using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class GetAllReposRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (GetAllReposRequest) request;
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            var reposList = reposService.GetAll(typedRequest.User, typedRequest.Page);

            return new GetAllReposResponse(reposList);
        }
    }
}