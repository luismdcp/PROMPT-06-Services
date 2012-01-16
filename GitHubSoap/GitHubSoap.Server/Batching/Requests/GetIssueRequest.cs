using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class GetIssueRequest : Request
    {
        [DataMember]
        public string Repo { get; set; }

        [DataMember]
        public int Number { get; set; }
    }
}