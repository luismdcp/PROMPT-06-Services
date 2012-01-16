using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Batching.Responses
{
    [DataContract]
    public class GetAllReposResponse : Response
    {
        [DataMember]
        public IList<Repo> ReposList { get; set; }

        public GetAllReposResponse(IList<Repo> reposList)
        {
            this.ReposList = reposList;
        }
    }
}