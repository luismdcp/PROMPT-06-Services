using System;
using System.Collections.Generic;
using GitHubSoap.Domain.Issues;
using GitHubSoap.Repositories.Contracts;
using GitHubSoap.Services.Contracts;

namespace GitHubSoap.Services.Implementation
{
    public class IssuesService : IIssuesService
    {
        private IIssuesRepository issuesRepository;

        public IssuesService(IIssuesRepository issuesRepository)
        {
            this.issuesRepository = issuesRepository;
        }

        public IList<Issue> GetAll(string user, string repo, int page)
        {
            return this.issuesRepository.GetAll(user, repo, page);
        }

        public Issue Get(string user, string repo, int number)
        {
            return this.issuesRepository.Get(user, repo, number);
        }

        public Issue Edit(string user, string password, string repo, int id, IssueEdit editIssue)
        {
            return this.issuesRepository.Edit(user, password, repo, id, editIssue);
        }

        public Issue Create(string user, string password, string repo, IssueCreate createIssue)
        {
            return this.issuesRepository.Create(user, password, repo, createIssue);
        }
    }
}