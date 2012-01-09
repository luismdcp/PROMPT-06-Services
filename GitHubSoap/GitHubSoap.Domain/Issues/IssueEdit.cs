using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class IssueEdit : IssueCreate
    {
        #region Properties

        [DataMember]
        public string state { get; set; }

        #endregion
    }
}