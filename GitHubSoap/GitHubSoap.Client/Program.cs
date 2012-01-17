using System.Configuration;
using System.ServiceModel;
using GitHubSoap.Client.Inspectors;
using GitHubSoap.Domain.Issues;
using GitHubSoap.Domain.Repos;
using GitHubSoap.Server.Contracts;

namespace GitHubSoap.Client
{
    public class Program
    {
        public static void Main()
        {
            // Settings needed to connect and use the Batching Service.
            string user = ConfigurationManager.AppSettings["User"];
            string password = ConfigurationManager.AppSettings["Password"];
            string serviceEndpointAddress = ConfigurationManager.AppSettings["ServiceEndpointAddress"];

            // Creation of a channel connected to the Regular Service.
            var endpoint = new EndpointAddress(serviceEndpointAddress);
            var binding = new BasicHttpBinding();
            ChannelFactory<IGitHubSoapService> channelFactory = new ChannelFactory<IGitHubSoapService>(binding, endpoint);
            channelFactory.Endpoint.Behaviors.Add(new AuthenticationHeaderBehavior());
            var serviceChannel = channelFactory.CreateChannel();

            // Create a new repository.
            var newRepo = new RepoCreate
                              {
                                  name = "Test-Repository",
                                  description = "Just a test repository",
                                  has_downloads = true,
                                  has_issues = true,
                                  has_wiki = false,
                                  @private = false
                              };

            var createdRepo = serviceChannel.CreateRepo(user, password, newRepo);

            // Get the created repository.
            var repo = serviceChannel.GetRepo(user, "Test-Repository");

            // Edit the repository.
            var editRepo = new RepoEdit {has_wiki = true, name = "Test-Repository"};
            serviceChannel.EditRepo(user, password, "Test-Repository", editRepo);

            // Create an issue in the created repository.
            var newIssue = new IssueCreate {title = "Found a bug", body = "I'm having a problem with this.", assignee = "luismdcp"};
            var createdIssue = serviceChannel.CreateIssue(user, password, "Test-Repository", newIssue);

            // Edit the created issue.
            var editIssue = new IssueEdit {milestone = 1};
            serviceChannel.EditIssue(user, password, "Test-Repository", createdIssue.id, editIssue);
        }
    }
}