using GitHubSoap.Domain.Users;
using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Repos
{
    [DataContract]
    public class Repo
    {
        #region Properties

        [DataMember]
        public string language { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string pushed_at { get; set; }

        [DataMember]
        public int watchers { get; set; }

        [DataMember]
        public int open_issues { get; set; }

        [DataMember]
        public string created_at { get; set; }

        [DataMember]
        public int forks { get; set; }

        [DataMember]
        public string homepage { get; set; }

        [DataMember]
        public int size { get; set; }

        [DataMember]
        public User owner { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string html_url { get; set; }

        [DataMember]
        public string updated_at { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string url { get; set; }

        #endregion
    }
}