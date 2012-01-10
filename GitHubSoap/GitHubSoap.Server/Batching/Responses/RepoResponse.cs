using System.Runtime.Serialization;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Batching.Responses
{
    [DataContract]
    public class RepoResponse : Response
    {
        public Repo SingleRepo { get; set; }

        public RepoResponse(Repo singleRepo)
        {
            this.SingleRepo = singleRepo;
        }
    }
}