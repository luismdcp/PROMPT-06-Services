using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using GitHubSoap.IoC;
using GitHubSoap.Server.Contracts;
using GitHubSoap.Server.Implementation;
using GitHubSoap.Server.Inspectors.Authentication;
using GitHubSoap.Server.Inspectors.CallsRateControl;
using GitHubSoap.Server.Inspectors.Authorization;

namespace GitHubSoap.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ContainerBootstrapper.BootstrapStructureMap();
            string serviceBaseURI = ConfigurationManager.AppSettings["ServiceBaseURI"];
            string batchingServiceBaseURI = ConfigurationManager.AppSettings["BatchingServiceBaseURI"];

            // Build and start the Regular Service.
            using (var host = new ServiceHost(typeof(GitHubSoapService), new Uri(serviceBaseURI)))
            {
                host.AddServiceEndpoint(typeof(IGitHubSoapService), new BasicHttpBinding(), "GitHubSoap");

                host.Description.Behaviors.Add(new AuthenticationHeaderBehavior());
                host.Description.Behaviors.Add(new RequestCallRateBehavior());
                host.Description.Behaviors.Add(new ServiceMetadataBehavior
                                                {
                                                    HttpGetEnabled = true,
                                                    HttpGetUrl = new Uri(serviceBaseURI + "/GitHubSoap/metadata")
                                                });

                host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();
                host.Open();
                Console.WriteLine("Regular Host is opened, press any key to end ...");

                // Build and start the Batching Service.
                using (var batchingHost = new ServiceHost(typeof(GitHubSoapBatchingService), new Uri(batchingServiceBaseURI)))
                {
                    batchingHost.AddServiceEndpoint(typeof(IGitHubSoapBatchingService), new BasicHttpBinding(), "GitHubSoapBatching");

                    batchingHost.Description.Behaviors.Add(new ServiceMetadataBehavior
                                                               {
                                                                HttpGetEnabled = true,
                                                                HttpGetUrl = new Uri(batchingServiceBaseURI + "/GitHubSoapBatching/metadata")
                                                            });

                    batchingHost.Open();
                    Console.WriteLine("Batching Host is opened, press any key to end ...");
                    Console.ReadKey();
                }
            }
        }
    }
}