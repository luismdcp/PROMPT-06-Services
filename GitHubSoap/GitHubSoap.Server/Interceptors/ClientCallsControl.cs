using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.ServiceModel;

namespace GitHubSoap.Server.Interceptors
{
    public static class ClientCallsControl
    {
        private static ConcurrentDictionary<string, ClientCallsInfo> CallsData;

        static ClientCallsControl()
        {
            CallsData = new ConcurrentDictionary<string, ClientCallsInfo>();
        }

        public static void IncrementOrAddClientCalls(string clientIPAddress)
        {
            if (CallsData.ContainsKey(clientIPAddress))
            {
                ClientCallsInfo callsInfo = null;
                CallsData.TryGetValue(clientIPAddress, out callsInfo);

                if (callsInfo != null)
                {
                    TimeSpan timeElapsed = DateTime.Now - callsInfo.TimeFirstCall;
                    int callsPerHour = Convert.ToInt32(ConfigurationSettings.AppSettings["CallsPerHour"]);

                    lock (callsInfo)
                    {
                        if (timeElapsed.Minutes <= 60 && callsInfo.CountCalls <= callsPerHour)
                        {
                            callsInfo.CountCalls++;
                        }
                        else
                        {
                            throw new FaultException(String.Format("Calls from your IP Address '{0}' exceeded the maximum number '{1}' per hour. Request Denied.", clientIPAddress, callsPerHour));
                        }
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