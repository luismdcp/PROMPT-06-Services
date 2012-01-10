using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class CreateIssueRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (CreateIssueRequest) request;
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            var issueResult = issuesService.Create(typedRequest.User, typedRequest.Password, typedRequest.Repo, typedRequest.CreateIssue);

            return new IssueResponse(issueResult);
        }
    }
}