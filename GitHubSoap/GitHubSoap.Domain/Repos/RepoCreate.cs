using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Repos
{
    [DataContract]
    public class RepoCreate : BaseRepo
    {
        [DataMember]
        public bool @private { get; set; }
    }
}