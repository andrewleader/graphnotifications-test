using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GraphNotificationsTest.Helpers
{
    public class GraphSdkHelper : IGraphSdkHelper
    {
        private readonly IGraphAuthProvider _authProvider;
        private GraphServiceClient _graphClient;

        public GraphSdkHelper(IGraphAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        // Get an authenticated Microsoft Graph Service client.
        public GraphServiceClient GetAuthenticatedClient(string userId)
        {
            _graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                    // Passing tenant ID to the sample auth provider to use as a cache key
                    var accessToken = await _authProvider.GetUserAccessTokenAsync(userId);

                    // Append the access token to the request
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // This header identifies the sample in the Microsoft Graph service. If extracting this code for your project please remove.
                    requestMessage.Headers.Add("SampleID", "aspnetcore-connect-sample");
                }));

            // Change to beta
            _graphClient.BaseUrl = "https://graph.microsoft.com/beta";

            return _graphClient;
        }

        public GraphServiceClient GetAuthenticatedClient(ClaimsPrincipal user)
        {
            string identifier = user.FindFirst(Startup.ObjectIdentifierType)?.Value;

            return GetAuthenticatedClient(identifier);
        }
    }
    public interface IGraphSdkHelper
    {
        GraphServiceClient GetAuthenticatedClient(string userId);

        GraphServiceClient GetAuthenticatedClient(ClaimsPrincipal user);
    }
}
