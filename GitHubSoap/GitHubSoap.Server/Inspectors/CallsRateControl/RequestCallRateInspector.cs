using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using GitHubSoap.Server.Inspectors.CallsRateControl;

namespace GitHubSoap.Server.Inspectors.CallsRateControl
{
    public class RequestCallRateInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            RemoteEndpointMessageProperty remoteEndpoint = request.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            IPAddress address = IPAddress.Parse(remoteEndpoint.Address);
            ClientCallsRateControl.IncrementOrAddClientCalls(address.ToString());

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {

        }
    }
}