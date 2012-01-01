using System.Collections.Generic;

namespace GitHubSoap.Domain.Issues
{
    public class IssueEdit : IssueCreate
    {
        #region Properties

        public IList<Label> labels { get; set; }

        #endregion
    }
}