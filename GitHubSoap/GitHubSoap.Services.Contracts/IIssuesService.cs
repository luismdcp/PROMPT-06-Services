﻿using System.Collections.Generic;
using GitHubSoap.Domain.Issues;

namespace GitHubSoap.Services.Contracts
{
    public interface IIssuesService
    {
        IList<Issue> GetAll(string user, string repo, int page);
        Issue Get(string user, string repo, int number);
        Issue Edit(string user, string password, string repo, int id, IssueEdit editIssue);
        Issue Create(string user, string password, string repo, IssueCreate createIssue);
    }
}