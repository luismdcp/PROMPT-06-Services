using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Repos
{
    [DataContract]
    public class BaseRepo
    {
        #region Properties

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string homepage { get; set; }

        [DataMember]
        public bool has_issues { get; set; }

        [DataMember]
        public bool has_wiki { get; set; }

        [DataMember]
        public bool has_downloads { get; set; }

        #endregion
    }
}