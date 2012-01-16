using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using GitHubSoap.Domain.Repos;
using GitHubSoap.Repositories.Contracts;

namespace GitHubSoap.Repositories.REST
{
    public class ReposRepository : IReposRepository
    {
        public IList<Repo> GetAll(string user, int page)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/users/{0}/repos?page={1}", user, page);

            var response = client.GetAsync(uri).Result;
            var result = response.Content.ReadAsAsync<IList<Repo>>().Result;

            return result;
        }

        public Repo Get(string user, string repo)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/repos/{0}/{1}", user, repo);

            var response = client.GetAsync(uri).Result;
            var result = response.Content.ReadAsAsync<Repo>().Result;

            return result;
        }

        public Repo Edit(string user, string password, string repo, RepoEdit editRepo)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/repos/{0}/{1}", user, repo);
            var request = new HttpRequestMessage<RepoEdit>(editRepo,
                                                                        new HttpMethod("PATCH"),
                                                                        new Uri(uri),
                                                                        new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthentication(user, password);
            var response = client.SendAsync(request).Result;
            var result = response.Content.ReadAsAsync<Repo>().Result;

            return result;
        }

        public Repo Create(string user, string password, RepoCreate createRepo)
        {
            var client = new HttpClient();
            var uri = String.Format("https://api.github.com/user/repos");
            var request = new HttpRequestMessage<RepoCreate>(createRepo,
                                                                           new HttpMethod("Post"),
                                                                           new Uri(uri),
                                                                           new List<MediaTypeFormatter> { new JsonMediaTypeFormatter() });

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = CreateBasicAuthentication(user, password);
            var response = client.SendAsync(request).Result;
            var result = response.Content.ReadAsAsync<Repo>().Result;

            return result;
        }

        private static AuthenticationHeaderValue CreateBasicAuthentication(string userName, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes(userName + ":" + password);
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
    }
}