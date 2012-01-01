using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHubSoap.Domain.Users;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class Issue
    {
        #region Properties

        [DataMember]
        public IList<Label> labels { get; set; }

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string body { get; set; }

        [DataMember]
        public User assignee { get; set; }

        [DataMember]
        public string state { get; set; }

        [DataMember]
        public string created_at { get; set; }

        [DataMember]
        public int comments { get; set; }

        [DataMember]
        public string closed_at { get; set; }

        [DataMember]
        public int number { get; set; }

        [DataMember]
        public string updated_at { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public User user { get; set; }

        [DataMember]
        public string html_url { get; set; }

        #endregion
    }
}