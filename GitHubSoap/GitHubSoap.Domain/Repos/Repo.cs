using GitHubSoap.Domain.Users;

namespace GitHubSoap.Domain.Repos
{
    public class Repo
    {
        #region Properties

        public string language { get; set; }
        public string description { get; set; }
        public string pushed_at { get; set; }
        public int watchers { get; set; }
        public int open_issues { get; set; }
        public string created_at { get; set; }
        public int forks { get; set; }
        public string homepage { get; set; }
        public int size { get; set; }
        public User owner { get; set; }
        public string name { get; set; }
        public string html_url { get; set; }
        public string updated_at { get; set; }
        public int id { get; set; }
        public string url { get; set; }

        #endregion
    }
}