using System.Runtime.Serialization;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class EditIssueRequest : Request
    {
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Repo { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public IssueEdit EditIssue { get; set; }
    }
}