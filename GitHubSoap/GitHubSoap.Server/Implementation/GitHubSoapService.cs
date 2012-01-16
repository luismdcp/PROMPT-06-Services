using System.Collections.Generic;
using System.ServiceModel;
using GitHubSoap.Domain.Issues;
using GitHubSoap.Domain.Repos;
using GitHubSoap.Server.Contracts;
using GitHubSoap.Services.Contracts;
using StructureMap;
using GitHubSoap.Server.Inspectors.Authorization;

namespace GitHubSoap.Server.Implementation
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public class GitHubSoapService : IGitHubSoapService
    {
        public IList<Issue> GetAllIssues(string user, string repo, int page)
        {
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            return issuesService.GetAll(user, repo, page);
        }

        public Issue GetIssue(string user, string repo, int number)
        {
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            return issuesService.Get(user, repo, number);
        }

        public Issue EditIssue(string user, string password, string repo, int id, IssueEdit editIssue)
        {
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            return issuesService.Edit(user, password, repo, id, editIssue);
        }

        public Issue CreateIssue(string user, string password, string repo, IssueCreate createIssue)
        {
            IIssuesService issuesService = ObjectFactory.GetInstance<IIssuesService>();
            return issuesService.Create(user, password, repo, createIssue);
        }

        public IList<Repo> GetAllRepos(string user, int page)
        {
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            return reposService.GetAll(user, page);
        }

        public Repo GetRepo(string user, string repo)
        {
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            return reposService.Get(user, repo);
        }

        public Repo EditRepo(string user, string password, string repo, RepoEdit editRepo)
        {
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            return reposService.Edit(user, password, repo, editRepo);
        }

        public Repo CreateRepo(string user, string password, RepoCreate createRepo)
        {
            IReposService reposService = ObjectFactory.GetInstance<IReposService>();
            return reposService.Create(user, password, createRepo);
        }
    }
}