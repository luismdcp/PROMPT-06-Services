using System.ServiceModel;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Server.Implementation;

namespace GitHubSoap.Server.Contracts
{
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(KnownTypeProvider))]
    public interface IGitHubSoapBatchingService
    {
        [OperationContract]
        Response[] Process(params Request[] requests);
    }
}