using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Repos
{
    [DataContract]
    public class RepoEdit : BaseRepo
    {
        [DataMember]
        public bool @public { get; set; }
    }
}