using System.Collections.Generic;
using System.ServiceModel;
using GitHubSoap.Domain.Issues;
using GitHubSoap.Domain.Repos;

namespace GitHubSoap.Server.Contracts
{
    [ServiceContract]
    public interface IGitHubSoapService
    {
        #region Issues Operations

        [OperationContract]
        IList<Issue> GetAllIssues(string user, string repo, int page);

        [OperationContract]
        Issue GetIssue(string user, string repo, int number);

        [OperationContract]
        Issue EditIssue(string user, string password, string repo, int id, IssueEdit editIssue);

        [OperationContract]
        Issue CreateIssue(string user, string password, string repo, IssueCreate createIssue);

        #endregion

        #region Repos Operations

        [OperationContract]
        IList<Repo> GetAllRepos(string user, int page);

        [OperationContract]
        Repo GetRepo(string user, string repo);

        [OperationContract]
        Repo EditRepo(string user, string password, string repo, RepoEdit editRepo);

        [OperationContract]
        Repo CreateRepo(string user, string password, RepoCreate createRepo);

        #endregion
    }
}