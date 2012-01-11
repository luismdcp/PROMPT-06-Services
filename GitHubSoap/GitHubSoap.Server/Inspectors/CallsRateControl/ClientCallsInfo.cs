using System;

namespace GitHubSoap.Server.Inspectors.CallsRateControl
{
    public class ClientCallsInfo
    {
        public int CountCalls { get; set; }
        public DateTime TimeFirstCall { get; set; }

        public ClientCallsInfo()
        {
            this.CountCalls = 1;
            this.TimeFirstCall = DateTime.Now;
        }
    }
}