using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class GetAllIssuesRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (GetAllIssuesRequest) request;
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            var issuesResult = issuesService.GetAll(typedRequest.User, typedRequest.Repo, typedRequest.Page);

            return new GetAllIssuesResponse(issuesResult);
        }
    }
}