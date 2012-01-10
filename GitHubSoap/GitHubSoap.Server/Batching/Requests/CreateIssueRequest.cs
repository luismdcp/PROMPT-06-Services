using System.Runtime.Serialization;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class CreateIssueRequest : Request
    {
        public string Password { get; set; }
        public string Repo { get; set; }
        public IssueCreate CreateIssue { get; set; }
    }
}