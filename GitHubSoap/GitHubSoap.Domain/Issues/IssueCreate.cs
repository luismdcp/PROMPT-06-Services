using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class IssueCreate
    {
        #region Properties

        [DataMember]
        public string title { get; set; }

        [DataMember]
        public string body { get; set; }

        [DataMember]
        public string assignee { get; set; }

        [DataMember]
        public int milestone { get; set; }

        [DataMember]
        public IList<Label> labels { get; set; }

        #endregion
    }
}