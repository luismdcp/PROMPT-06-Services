using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class GetIssueRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (GetIssueRequest) request;
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            var issueResult = issuesService.Get(typedRequest.User, typedRequest.Repo, typedRequest.Number);

            return new IssueResponse(issueResult);
        }
    }
}