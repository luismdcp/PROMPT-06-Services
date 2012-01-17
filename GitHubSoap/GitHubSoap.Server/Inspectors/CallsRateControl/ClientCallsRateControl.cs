using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.ServiceModel;

namespace GitHubSoap.Server.Inspectors.CallsRateControl
{
    public static class ClientCallsRateControl
    {
        private static readonly ConcurrentDictionary<string, ClientCallsInfo> CallsData;

        static ClientCallsRateControl()
        {
            CallsData = new ConcurrentDictionary<string, ClientCallsInfo>();
        }

        public static void IncrementOrAddClientCalls(string clientIPAddress)
        {
            if (CallsData.ContainsKey(clientIPAddress))
            {
                ClientCallsInfo callsInfo;
                CallsData.TryGetValue(clientIPAddress, out callsInfo);

                TimeSpan timeElapsed = DateTime.Now - callsInfo.TimeFirstCall;
                int callsPerHour = Convert.ToInt32(ConfigurationManager.AppSettings["CallsPerHour"]);
                int timeIntervalInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["TimeIntervalInMinutes"]);

                lock (callsInfo)
                {
                    if (timeElapsed.Minutes <= timeIntervalInMinutes)
                    {
                        if (callsInfo.CountCalls >= callsPerHour)
                        {
                            throw new FaultException(String.Format("Calls from your IP Address '{0}' exceeded the maximum number '{1}' per time interval for '{2}'. Request Denied.", clientIPAddress, callsPerHour, timeIntervalInMinutes));
                        }
                        else
                        {
                            callsInfo.CountCalls++;
                        }
                    }
                    else
                    {
                        callsInfo.CountCalls = 1;
                    }
                }
            }
            else
            {
                CallsData.TryAdd(clientIPAddress, new ClientCallsInfo());
            }
        }
    }
}