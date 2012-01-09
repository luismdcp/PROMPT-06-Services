using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace GitHubSoap.Server.Interceptors
{
    public class RequestInterceptor : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            RemoteEndpointMessageProperty remoteEndpoint = request.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            IPAddress address = IPAddress.Parse(remoteEndpoint.Address);
            ClientCallsControl.IncrementOrAddClientCalls(address.ToString());

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {

        }
    }
}