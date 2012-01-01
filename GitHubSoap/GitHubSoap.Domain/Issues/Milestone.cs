using System;
using GitHubSoap.Domain.Users;

namespace GitHubSoap.Domain.Issues
{
    public class Milestone
    {
        #region Properties

        public int closed_issues { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string state { get; set; }
        public string created_at { get; set; }
        public int open_issues { get; set; }
        public int number { get; set; }
        public User creator { get; set; }
        public string url { get; set; }

        #endregion
    }
}