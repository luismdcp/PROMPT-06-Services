using System.Runtime.Serialization;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class EditRepoRequest : Request
    {
        public string Password { get; set; }
        public string Repo { get; set; }
        public RepoEdit EditRepo { get; set; }
    }
}