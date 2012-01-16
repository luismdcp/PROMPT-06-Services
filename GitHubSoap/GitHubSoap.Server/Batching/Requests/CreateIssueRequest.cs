using System.Runtime.Serialization;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class CreateIssueRequest : Request
    {
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Repo { get; set; }

        [DataMember]
        public IssueCreate CreateIssue { get; set; }
    }
}