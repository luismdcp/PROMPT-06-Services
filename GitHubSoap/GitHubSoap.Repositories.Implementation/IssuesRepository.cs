using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using GitHubSoap.Domain.Issues;
using GitHubSoap.Repositories.Contracts;
using System.Net;

namespace GitHubSoap.Repositories.REST
{
    public class IssuesRepository : IIssuesRepository
    {
        public IList<Issue> GetAll(string user, string repo, int page)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/repos/{0}/{1}/issues?page={2}", user, repo, page);

            var response = client.GetAsync(uri).Result;
            var result = response.Content.ReadAsAsync<IList<Issue>>().Result;

            return result;
        }

        public Issue Get(string user, string repo, int number)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/repos/{0}/{1}/issues/{2}", user, repo, number);

            var response = client.GetAsync(uri).Result;
            var result = response.Content.ReadAsAsync<Issue>().Result;

            return result;
        }

        public Issue Edit(string user, string password, string repo, int id, IssueEdit editIssue)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/repos/{0}/{1}/issues/{2}", user, repo, id);
            var request = new HttpRequestMessage<IssueEdit>(editIssue,
                                                                        new HttpMethod("PATCH"),
                                                                        new Uri(uri),
                                                                        new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthentication(user, password);
            var response = client.SendAsync(request).Result;
            var result = response.Content.ReadAsAsync<Issue>().Result;

            return result;
        }

        public Issue Create(string user, string password, string repo, IssueCreate createIssue)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/repos/{0}/{1}/issues", user, repo);
            var request = new HttpRequestMessage<IssueCreate>(createIssue,
                                                                           new HttpMethod("POST"),
                                                                           new Uri(uri),
                                                                           new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthentication(user, password);
            var response = client.SendAsync(request).Result;
            var t = response.Content.ReadAsAsync<WebException>().Result;
            var result = response.Content.ReadAsAsync<Issue>().Result;

            return result;
        }

        private static AuthenticationHeaderValue CreateBasicAuthentication(string userName, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes(userName + ":" + password);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}