using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace GitHubSoap.Server.Interceptors
{
    public class RequestsInterceptionBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
             
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            RequestInterceptor interceptor = new RequestInterceptor();
            ChannelDispatcher endpointDispatcher = serviceHostBase.ChannelDispatchers[0] as ChannelDispatcher;

            endpointDispatcher.Endpoints[0].DispatchRuntime.MessageInspectors.Add(interceptor);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}