using System;
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

            using (var host = new ServiceHost(typeof(GitHubSoapService), new Uri("http://localhost:8080")))
            {
                host.AddServiceEndpoint(typeof(IGitHubSoapService), new BasicHttpBinding(), "GitHubSoap");

                host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                                                {
                                                    HttpGetEnabled = true,
                                                    HttpGetUrl = new Uri("http://localhost:8080/metadata")
                                                });

                host.Open();
                Console.WriteLine("Host is opened, press any key to end ...");
            }
        }
    }
}