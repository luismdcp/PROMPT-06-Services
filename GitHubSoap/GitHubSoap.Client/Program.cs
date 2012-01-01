using System.ServiceModel;
using GitHubSoap.Domain.Issues;
using GitHubSoap.Domain.Repos;
using GitHubSoap.Server.Contracts;

namespace GitHubSoap.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string user = "luismdcp";
            const string password = "Rah-X3ph0n";

            var endpoint = new EndpointAddress("http://localhost:8080/GitHubSoap");
            var binding = new BasicHttpBinding();
            ChannelFactory<IGitHubSoapService> channelFactory = new ChannelFactory<IGitHubSoapService>(binding, endpoint);
            var serviceChannel = channelFactory.CreateChannel();

            var newRepo = new RepoCreate();
            newRepo.name = "Test Repository";
            newRepo.description = "Just a test repository";
            newRepo.has_downloads = true;
            newRepo.has_issues = true;
            newRepo.has_wiki = false;
            newRepo.@private = false;

            var createdRepo = serviceChannel.CreateRepo(user, password, newRepo);
            var repo = serviceChannel.GetRepo(user, "Test Repository");

            var editRepo = new RepoEdit();
            editRepo.has_wiki = true;

            serviceChannel.EditRepo(user, password, "Test repository", editRepo);

            var newIssue = new IssueCreate();
            newIssue.title = "Found a bug";
            newIssue.body = "I'm having a problem with this.";
            newIssue.assignee = "luismdcp";

            var createdIssue = serviceChannel.CreateIssue(user, password, "Test Repository", newIssue);

            var editIssue = new IssueEdit();
            editIssue.milestone = 1;

            serviceChannel.EditIssue(user, password, "Test Repository", createdIssue.id, editIssue);
        }
    }
}