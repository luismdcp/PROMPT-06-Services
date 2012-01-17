using System.Configuration;
using System.ServiceModel;
using GitHubSoap.Domain.Repos;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;
using GitHubSoap.Server.Contracts;

namespace GitHubSoap.Client.Batching
{
    public class Program
    {
        public static void Main()
        {
            // Settings needed to connect and use the Batching Service.
            string user = ConfigurationManager.AppSettings["User"];
            string password = ConfigurationManager.AppSettings["Password"];
            string batchingServiceEndpointAddress = ConfigurationManager.AppSettings["BatchingServiceEndpointAddress"];

            // Creation of a channel connected to the Batching Service.
            var endpoint = new EndpointAddress(batchingServiceEndpointAddress);
            var binding = new BasicHttpBinding();
            ChannelFactory<IGitHubSoapBatchingService> channelFactory = new ChannelFactory<IGitHubSoapBatchingService>(binding, endpoint);
            var serviceChannel = channelFactory.CreateChannel();

            // Build a Request to create a new repository.
            var newRepo = new RepoCreate
                              {
                                  name = "Another-Test-Repository",
                                  description = "Just another test repository",
                                  has_downloads = true,
                                  has_issues = true,
                                  has_wiki = false,
                                  @private = false
                              };

            var createRepoRequest = new CreateRepoRequest {User = user, Password = password, CreateRepo = newRepo};

            // Build a Request to get the created repository.
            var getRepoRequest = new GetRepoRequest {User = user, Repo = newRepo.name};

            // Build a request to edit the created repository.
            var editRepo = new RepoEdit {has_wiki = true, name = newRepo.name};

            var editRepoRequest = new EditRepoRequest {User = user, Password = password, Repo = newRepo.name, EditRepo = editRepo};

            // Call the Batching Service.
            var results = serviceChannel.Process(createRepoRequest, getRepoRequest, editRepoRequest);

            // Get the indexed responses.
            var createRepoResponse = (RepoResponse) results[0];
            var getRepoResponse = (RepoResponse) results[1];
            var editRepoResponse = (RepoResponse) results[2];
        }
    }
}