using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Server.Batching.Responses
{
    [DataContract]
    public class GetAllIssuesResponse : Response
    {
        [DataMember]
        public IList<Issue> IssuesList { get; set; }

        public GetAllIssuesResponse(IList<Issue> issuesList)
        {
            this.IssuesList = issuesList;
        }
    }
}