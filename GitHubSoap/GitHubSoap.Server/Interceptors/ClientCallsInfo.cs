using System;

namespace GitHubSoap.Server.Interceptors
{
    public class ClientCallsInfo
    {
        public int CountCalls { get; set; }
        public DateTime TimeFirstCall { get; set; }

        public ClientCallsInfo()
        {
            this.CountCalls = 0;
            this.TimeFirstCall = DateTime.Now;
        }
    }
}