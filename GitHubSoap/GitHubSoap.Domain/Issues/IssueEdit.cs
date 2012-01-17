using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class IssueEdit : IssueCreate
    {
        [DataMember]
        public string state { get; set; }
    }
}