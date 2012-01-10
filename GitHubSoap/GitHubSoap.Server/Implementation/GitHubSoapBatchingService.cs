using System;
using System.Collections.Generic;
using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Handlers.Implementation;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Server.Contracts;

namespace GitHubSoap.Server.Implementation
{
    public class GitHubSoapBatchingService : IGitHubSoapBatchingService
    {
        private static readonly Dictionary<Type, IRequestHandler> requestTypesToRequestHandlerTypes;

        static GitHubSoapBatchingService()
        {
            requestTypesToRequestHandlerTypes = new Dictionary<Type, IRequestHandler>();
            requestTypesToRequestHandlerTypes.Add(typeof(CreateIssueRequest), new CreateIssueRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(CreateRepoRequest), new CreateRepoRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(EditIssueRequest), new EditIssueRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(EditRepoRequest), new EditRepoRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(GetAllIssuesRequest), new GetAllIssuesRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(GetAllReposRequest), new GetAllReposRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(GetIssueRequest), new GetIssueRequestHandler());
            requestTypesToRequestHandlerTypes.Add(typeof(GetRepoRequest), new GetRepoRequestHandler());
        }

        public Response[] Process(params Request[] requests)
        {
            var responses = new List<Response>();

            foreach (var request in requests)
            {
                var requestType = request.GetType();
                var handlerType = requestTypesToRequestHandlerTypes[requestType];

                responses.Add(handlerType.Handle(request));
            }

            return responses.ToArray();
        }
    }
}