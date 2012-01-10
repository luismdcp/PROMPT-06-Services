using System.Configuration;
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
            // Settings needed to connect and use the Batching Service.
            string user = ConfigurationSettings.AppSettings["User"];
            string password = ConfigurationSettings.AppSettings["Password"];
            string serviceEndpointAddress = ConfigurationSettings.AppSettings["ServiceEndpointAddress"];

            // Creation of a channel connected to the Regular Service.
            var endpoint = new EndpointAddress(serviceEndpointAddress);
            var binding = new BasicHttpBinding();
            ChannelFactory<IGitHubSoapService> channelFactory = new ChannelFactory<IGitHubSoapService>(binding, endpoint);
            var serviceChannel = channelFactory.CreateChannel();

            // Create a new repository.
            var newRepo = new RepoCreate();
            newRepo.name = "Test-Repository";
            newRepo.description = "Just a test repository";
            newRepo.has_downloads = true;
            newRepo.has_issues = true;
            newRepo.has_wiki = false;
            newRepo.@private = false;

            var createdRepo = serviceChannel.CreateRepo(user, password, newRepo);

            // Get the created repository.
            var repo = serviceChannel.GetRepo(user, "Test-Repository");

            // Edit the repository.
            var editRepo = new RepoEdit();
            editRepo.has_wiki = true;
            editRepo.name = "Test-Repository";

            serviceChannel.EditRepo(user, password, "Test-Repository", editRepo);

            // Create an issue in the created repository.
            var newIssue = new IssueCreate();
            newIssue.title = "Found a bug";
            newIssue.body = "I'm having a problem with this.";
            newIssue.assignee = "luismdcp";

            var createdIssue = serviceChannel.CreateIssue(user, password, "Test-Repository", newIssue);

            // Edit the created issue.
            var editIssue = new IssueEdit();
            editIssue.milestone = 1;

            serviceChannel.EditIssue(user, password, "Test-Repository", createdIssue.id, editIssue);
        }
    }
}