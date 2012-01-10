using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Services.Contracts;
using StructureMap;

namespace GitHubSoap.Server.Batching.Handlers.Implementation
{
    public class EditIssueRequestHandler : IRequestHandler
    {
        public Response Handle(Request request)
        {
            var typedRequest = (EditIssueRequest) request;
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            var issueResult = issuesService.Edit(typedRequest.User, typedRequest.Password, typedRequest.Repo, typedRequest.Id, typedRequest.EditIssue);

            return new IssueResponse(issueResult);
        }
    }
}