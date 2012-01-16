using System.Runtime.Serialization;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class CreateRepoRequest : Request
    {
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public RepoCreate CreateRepo { get; set; }
    }
}