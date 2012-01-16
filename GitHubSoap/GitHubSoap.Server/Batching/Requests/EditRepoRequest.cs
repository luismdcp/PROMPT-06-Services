using System.Runtime.Serialization;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class EditRepoRequest : Request
    {
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Repo { get; set; }

        [DataMember]
        public RepoEdit EditRepo { get; set; }
    }
}