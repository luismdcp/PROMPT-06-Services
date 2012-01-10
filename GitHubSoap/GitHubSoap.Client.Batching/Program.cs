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
        public static void Main(string[] args)
        {
            // Settings needed to connect and use the Batching Service.
            string user = ConfigurationSettings.AppSettings["User"];
            string password = ConfigurationSettings.AppSettings["Password"];
            string batchingServiceEndpointAddress = ConfigurationSettings.AppSettings["BatchingServiceEndpointAddress"];

            // Creation of a channel connected to the Batching Service.
            var endpoint = new EndpointAddress(batchingServiceEndpointAddress);
            var binding = new BasicHttpBinding();
            ChannelFactory<IGitHubSoapBatchingService> channelFactory = new ChannelFactory<IGitHubSoapBatchingService>(binding, endpoint);
            var serviceChannel = channelFactory.CreateChannel();

            // Build a Request to create a new repository.
            var newRepo = new RepoCreate();
            newRepo.name = "Another-Test-Repository";
            newRepo.description = "Just another test repository";
            newRepo.has_downloads = true;
            newRepo.has_issues = true;
            newRepo.has_wiki = false;
            newRepo.@private = false;

            var createRepoRequest = new CreateRepoRequest();
            createRepoRequest.User = user;
            createRepoRequest.Password = password;
            createRepoRequest.CreateRepo = newRepo;

            // Build a Request to get the created repository.
            var getRepoRequest = new GetRepoRequest();
            getRepoRequest.User = user;
            getRepoRequest.Repo = newRepo.name;

            // Build a request to edit the created repository.
            var editRepo = new RepoEdit();
            editRepo.has_wiki = true;
            editRepo.name = newRepo.name;

            var editRepoRequest = new EditRepoRequest();
            editRepoRequest.User = user;
            editRepoRequest.Password = password;
            editRepoRequest.Repo = newRepo.name;
            editRepoRequest.EditRepo = editRepo;

            // Call the Batching Service.
            var results = serviceChannel.Process(createRepoRequest, getRepoRequest, editRepoRequest);

            // Get the indexed responses.
            var createRepoResponse = (RepoResponse) results[0];
            var getRepoResponse = (RepoResponse) results[1];
            var editRepoResponse = (RepoResponse) results[2];
        }
    }
}