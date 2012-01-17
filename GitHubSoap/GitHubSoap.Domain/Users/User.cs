using System.Runtime.Serialization;

namespace GitHubSoap.Domain.Users
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string login { get; set; }

        [DataMember]
        public string url { get; set; }

        [DataMember]
        public int id { get; set; }
    }
}