using System.Runtime.Serialization;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Server.Batching.Responses
{
    [DataContract]
    public class IssueResponse : Response
    {
        [DataMember]
        public Issue SingleIssue { get; set; }

        public IssueResponse(Issue singleIssue)
        {
            this.SingleIssue = singleIssue;
        }
    }
}