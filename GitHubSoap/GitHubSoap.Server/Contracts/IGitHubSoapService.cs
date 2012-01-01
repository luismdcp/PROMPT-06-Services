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

        [OperationContract(Name = "GetAllIssues")]
        IList<Issue> GetAll(string user, string repo, int page);

        [OperationContract(Name = "GetIssue")]
        Issue Get(string user, string repo, int number);

        [OperationContract(Name = "EditIssue")]
        Issue Edit(string user, string password, string repo, int id, IssueEdit editIssue);

        [OperationContract(Name = "CreateIssue")]
        Issue Create(string user, string password, string repo, IssueCreate createIssue);

        #endregion

        #region Repos Operations

        [OperationContract(Name = "GetAllRepos")]
        IList<Repo> GetAll(string user, int page);

        [OperationContract(Name = "GetRepo")]
        Repo Get(string user, string repo);

        [OperationContract(Name = "EditRepo")]
        Repo Edit(string user, string password, string repo, RepoEdit editRepo);

        [OperationContract(Name = "CreateRepo")]
        Repo Create(string user, string password, RepoCreate createRepo);

        #endregion
    }
}