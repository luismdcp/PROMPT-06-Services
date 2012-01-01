using System.Runtime.Serialization;
using GitHubSoap.Domain.Users;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class Milestone
    {
        #region Properties

        [DataMember]
        public int closed_issues { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string created_at { get; set; }

        [DataMember]
        public int open_issues { get; set; }

        [DataMember]
        public int number { get; set; }

        [DataMember]
        public User creator { get; set; }

        [DataMember]
        public string url { get; set; }

        #endregion
    }
}