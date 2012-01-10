using System.Runtime.Serialization;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class CreateRepoRequest : Request
    {
        public string Password { get; set; }
        public RepoCreate CreateRepo { get; set; }
    }
}