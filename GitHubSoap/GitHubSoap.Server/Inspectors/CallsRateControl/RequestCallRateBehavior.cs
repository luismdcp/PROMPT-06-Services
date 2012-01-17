using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace GitHubSoap.Server.Inspectors.CallsRateControl
{
    public class RequestCallRateBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
             
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            RequestCallRateInspector interceptor = new RequestCallRateInspector();
            ChannelDispatcher endpointDispatcher = serviceHostBase.ChannelDispatchers[0] as ChannelDispatcher;

            if (endpointDispatcher != null)
            {
                endpointDispatcher.Endpoints[0].DispatchRuntime.MessageInspectors.Add(interceptor);
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}