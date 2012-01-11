using System.Collections.Generic;
using GitHubSoap.Domain.Repos;
using GitHubSoap.Repositories.Contracts;
using GitHubSoap.Services.Contracts;

namespace GitHubSoap.Services.Persistence
{
    public class ReposService : IReposService
    {
        private IReposRepository reposRepository;

        public ReposService(IReposRepository reposRepository)
        {
            this.reposRepository = reposRepository;
        }

        public IList<Repo> GetAll(string user, int page)
        {
            return this.reposRepository.GetAll(user, page);
        }

        public Repo Get(string user, string repo)
        {
            return this.reposRepository.Get(user, repo);
        }

        public Repo Edit(string user, string password, string repo, RepoEdit editRepo)
        {
            return this.reposRepository.Edit(user, password, repo, editRepo);
        }

        public Repo Create(string user, string password, RepoCreate createRepo)
        {
            return this.reposRepository.Create(user, password, createRepo);
        }
    }
}