using System;
using System.Collections.Generic;
using System.ServiceModel;
using GitHubSoap.Server.Batching.Handlers.Contracts;
using GitHubSoap.Server.Batching.Handlers.Implementation;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Server.Contracts;

namespace GitHubSoap.Server.Implementation
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class GitHubSoapBatchingService : IGitHubSoapBatchingService
    {
        private static readonly Dictionary<Type, IRequestHandler> RequestTypesToRequestHandlerTypes;

        static GitHubSoapBatchingService()
        {
            RequestTypesToRequestHandlerTypes = new Dictionary<Type, IRequestHandler>
                                                    {
                                                        {typeof (CreateIssueRequest), new CreateIssueRequestHandler()},
                                                        {typeof (CreateRepoRequest), new CreateRepoRequestHandler()},
                                                        {typeof (EditIssueRequest), new EditIssueRequestHandler()},
                                                        {typeof (EditRepoRequest), new EditRepoRequestHandler()},
                                                        {typeof (GetAllIssuesRequest), new GetAllIssuesRequestHandler()},
                                                        {typeof (GetAllReposRequest), new GetAllReposRequestHandler()},
                                                        {typeof (GetIssueRequest), new GetIssueRequestHandler()},
                                                        {typeof (GetRepoRequest), new GetRepoRequestHandler()}
                                                    };
        }

        public Response[] Process(params Request[] requests)
        {
            var responses = new List<Response>();

            foreach (var request in requests)
            {
                var requestType = request.GetType();
                var handlerType = RequestTypesToRequestHandlerTypes[requestType];

                responses.Add(handlerType.Handle(request));
            }

            return responses.ToArray();
        }
    }
}