using System;

namespace GitHubSoap.Server.Inspectors.CallsRateControl
{
    public class ClientCallsInfo
    {
        // Counter to get the total number of message requests
        public int CountCalls { get; set; }

        // Time when the client made the first request.
        public DateTime TimeFirstCall { get; private set; }

        public ClientCallsInfo()
        {
            this.CountCalls = 1;
            this.TimeFirstCall = DateTime.Now;
        }
    }
}