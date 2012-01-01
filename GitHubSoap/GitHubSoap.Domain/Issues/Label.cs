using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class Label
    {
        #region Properties

        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string url { get; set; }

        #endregion
    }
}