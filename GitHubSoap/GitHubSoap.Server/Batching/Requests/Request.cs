using System;
using System.Runtime.Serialization;

namespace GitHubSoap.Server.Batching.Requests
{
    [DataContract]
    public abstract class Request
    {
        public string User { get; set; }
    }
}