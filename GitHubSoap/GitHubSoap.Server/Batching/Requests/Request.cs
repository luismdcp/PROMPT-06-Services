using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public abstract class Request
    {
        [DataMember]
        public string User { get; set; }
    }
}