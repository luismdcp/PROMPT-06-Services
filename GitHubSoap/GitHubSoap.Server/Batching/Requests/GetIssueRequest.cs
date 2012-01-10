using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class GetIssueRequest : Request
    {
        public string Repo { get; set; }
        public int Number { get; set; }
    }
}