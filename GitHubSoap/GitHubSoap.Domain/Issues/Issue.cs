using System.Collections.Generic;
using GitHubSoap.Domain.Users;

namespace GitHubSoap.Domain.Issues
{
    public class Issue
    {
        #region Properties

        public IList<Label> labels { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public User assignee { get; set; }
        public string state { get; set; }
        public string created_at { get; set; }
        public int comments { get; set; }
        public string closed_at { get; set; }
        public int number { get; set; }
        public string updated_at { get; set; }
        public int id { get; set; }
        public string url { get; set; }
        public User user { get; set; }
        public string html_url { get; set; }

        #endregion
    }
}