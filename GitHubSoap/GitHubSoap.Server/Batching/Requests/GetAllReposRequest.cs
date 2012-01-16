using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public class GetAllReposRequest : Request
    {
        [DataMember]
        public int Page { get; set; }
    }
}