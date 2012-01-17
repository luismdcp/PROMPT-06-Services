using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using GitHubSoap.Security.Contracts;
using Microsoft.ApplicationServer.Http.Dispatcher;
using StructureMap;

namespace GitHubSoap.Server.Inspectors.Authentication
{
    public class AuthenticationHeaderInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var httpRequest = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            var authorizationHeader = httpRequest.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            string user;
            string password;

            this.ParseUserPasswordFromHeader(authorizationHeader, out user, out password);

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var authenticationService = ObjectFactory.GetInstance<IAuthenticationService>();

            if (!authenticationService.Authenticate(user, password))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return instanceContext;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            
        }

        private void ParseUserPasswordFromHeader(string authorizationHeader, out string user, out string password)
        {
            var headerValue = authorizationHeader.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var userAndPassword = encoding.GetString(Convert.FromBase64String(headerValue));
            var separator = userAndPassword.IndexOf(':');

            user = userAndPassword.Substring(0, separator);
            password = userAndPassword.Substring(separator + 1);
        }
    }
}