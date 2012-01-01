using System;
using System.Configuration;
using System.ServiceModel;
using Microsoft.ApplicationServer.Http;
using RESTBlogs.Server.Service;

namespace RESTBlogs.Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            string serviceURI = ConfigurationSettings.AppSettings["ServiceURI"];
            HttpServiceHost host = new HttpServiceHost(typeof(RESTBlogsService), serviceURI);

            try
            {
                host.Open();
                Console.WriteLine("Service is running...");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine("HttpServiceHost failed to open {0}", ex);
            }
            finally
            {
                if (host.State == CommunicationState.Faulted)
                {
                    host.Abort();
                }
                else
                {
                    host.Close();
                }
            }
        }
    }
}