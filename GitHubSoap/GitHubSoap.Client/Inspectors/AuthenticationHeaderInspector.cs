using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace GitHubSoap.Client.Inspectors
{
    public class AuthenticationHeaderInspector : IClientMessageInspector
    {
        public string User { get; set; }
        public string Password { get; set; }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var httpRequest = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            byte[] authenticationBytes = Encoding.ASCII.GetBytes(string.Concat(this.User, ":", this.Password));
            string base64 = Convert.ToBase64String(authenticationBytes);
            string authorization = string.Concat("Basic ", base64);
            httpRequest.Headers["authorization"] = authorization;

            return null;
        }
    }
}