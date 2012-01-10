using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using GitHubSoap.IoC;
using GitHubSoap.Server.Contracts;
using GitHubSoap.Server.Implementation;

namespace GitHubSoap.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ContainerBootstrapper.BootstrapStructureMap();
            string serviceBaseURI = ConfigurationSettings.AppSettings["ServiceBaseURI"];
            string batchingServiceBaseURI = ConfigurationSettings.AppSettings["BatchingServiceBaseURI"];

            // Build and start the Regular Service.
            using (var host = new ServiceHost(typeof(GitHubSoapService), new Uri(serviceBaseURI)))
            {
                host.AddServiceEndpoint(typeof(IGitHubSoapService), new BasicHttpBinding(), "GitHubSoap");

                host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                                                {
                                                    HttpGetEnabled = true,
                                                    HttpGetUrl = new Uri(serviceBaseURI + "/GitHubSoap/metadata")
                                                });

                host.Open();
                Console.WriteLine("Regular Host is opened, press any key to end ...");
            }

            // Build and start the Batching Service.
            using (var host = new ServiceHost(typeof(GitHubSoapBatchingService), new Uri(batchingServiceBaseURI)))
            {
                host.AddServiceEndpoint(typeof(IGitHubSoapBatchingService), new BasicHttpBinding(), "GitHubSoap");

                host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                                                {
                                                    HttpGetEnabled = true,
                                                    HttpGetUrl = new Uri(batchingServiceBaseURI + "/GitHubSoap/metadata")
                                                });

                host.Open();
                Console.WriteLine("Batching Host is opened, press any key to end ...");
                Console.ReadKey();
            }
        }
    }
}