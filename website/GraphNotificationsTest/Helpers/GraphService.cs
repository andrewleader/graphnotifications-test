using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GraphNotificationsTest.Helpers
{
    public static class GraphService
    {
        public static async Task PostNotificationAsync(GraphServiceClient graphClient, GraphNotification notification)
        {
            // Send to beta API: https://github.com/microsoftgraph/msgraph-sdk-dotnet/blob/dev/docs/overview.md#send-http-requests-with-the-net-microsoft-graph-client-library
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.microsoft.com/beta/me/notifications/");

            request.Content = new StringContent(JsonConvert.SerializeObject(notification), System.Text.Encoding.UTF8, "application/json");

            // Authenticate (add access token) our HttpRequestMessage
            await graphClient.AuthenticationProvider.AuthenticateRequestAsync(request);

            // Send the request and get the response.
            HttpResponseMessage response = await graphClient.HttpProvider.SendAsync(request);

            if (response.IsSuccessStatusCode && response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return;
            }
            else
            {
                throw new ServiceException(new Error()
                {
                    Code = response.StatusCode.ToString(),
                    Message = await response.Content.ReadAsStringAsync()
                });
            }
        }
    }
}
