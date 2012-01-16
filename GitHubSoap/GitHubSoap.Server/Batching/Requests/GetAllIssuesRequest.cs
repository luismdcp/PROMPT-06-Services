using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class GetAllIssuesRequest : Request
    {
        [DataMember]
        public string Repo { get; set; }

        [DataMember]
        public int Page { get; set; }
    }
}