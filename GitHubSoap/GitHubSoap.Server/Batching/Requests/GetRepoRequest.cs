using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class GetRepoRequest : Request
    {
        public string Repo { get; set; }
    }
}