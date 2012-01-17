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
            // if the client already made at least a request and his IP address is in the dictionary.
            if (CallsData.ContainsKey(clientIPAddress))
            {
                ClientCallsInfo callsInfo;
                CallsData.TryGetValue(clientIPAddress, out callsInfo);

                // get the time elapsed from the first request made.
                TimeSpan timeElapsed = DateTime.Now - callsInfo.TimeFirstCall;

                // get settings related to time interval and calls per hour.
                int callsPerHour = Convert.ToInt32(ConfigurationManager.AppSettings["CallsPerHour"]);
                int timeIntervalInMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["TimeIntervalInMinutes"]);

                // lock object in case multiple threads are updating the calls counter.
                lock (callsInfo)
                {
                    // if the time elapsed from the first call is less than the configurated time interval
                    if (timeElapsed.Minutes <= timeIntervalInMinutes)
                    {
                        // if the user exceeded the number of calls per hour.
                        if (callsInfo.CountCalls >= callsPerHour)
                        {
                            throw new FaultException(String.Format("Calls from your IP Address '{0}' exceeded the maximum number '{1}' per time interval for '{2}'. Request Denied.", clientIPAddress, callsPerHour, timeIntervalInMinutes));
                        }
                        else
                        {
                            // if the user has not exceeded increment with another call
                            callsInfo.CountCalls++;
                        }
                    }
                    else
                    {
                        // reset the counter if the configured time interval has elapsed.
                        callsInfo.CountCalls = 1;
                    }
                }
            }
            else
            {
                // if the user hasn't made any calls then add an object with the counter initialized to 1.
                CallsData.TryAdd(clientIPAddress, new ClientCallsInfo());
            }
        }
    }
}