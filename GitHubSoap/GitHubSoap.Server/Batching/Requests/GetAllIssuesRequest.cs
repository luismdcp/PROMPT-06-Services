using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class GetAllIssuesRequest : Request
    {
        public string Repo { get; set; }
        public int Page { get; set; }
    }
}