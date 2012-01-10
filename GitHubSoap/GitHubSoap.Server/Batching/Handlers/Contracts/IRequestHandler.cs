using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;

namespace GitHubSoap.Server.Batching.Handlers.Contracts
{
    public interface IRequestHandler
    {
        Response Handle(Request request);
    }
}