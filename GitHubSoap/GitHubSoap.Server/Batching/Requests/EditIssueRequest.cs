using System.Runtime.Serialization;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class EditIssueRequest : Request
    {
        public string Password { get; set; }
        public string Repo { get; set; }
        public int Id { get; set; }
        public IssueEdit EditIssue { get; set; }
    }
}