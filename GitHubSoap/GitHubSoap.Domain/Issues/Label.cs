using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Issues
{
    [DataContract]
    public class Label
    {
        [DataMember]
        public string color { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string url { get; set; }
    }
}