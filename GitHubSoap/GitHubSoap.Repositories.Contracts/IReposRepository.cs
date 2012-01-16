using System.Collections.Generic;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Repositories.Contracts
{
    public interface IReposRepository
    {
        IList<Repo> GetAll(string user, int page);
        Repo Get(string user, string repo);
        Repo Edit(string user, string password, string repo, RepoEdit editRepo);
        Repo Create(string user, string password, RepoCreate createRepo);
    }
}