using System.Collections.Generic;

namespace GitHubSoap.Domain.Issues
{
    public class IssueCreate
    {
        #region Properties

        public string title { get; set; }
        public string body { get; set; }
        public string assignee { get; set; }
        public int milestone { get; set; }
        public IList<Label> labels { get; set; }

        #endregion
    }
}