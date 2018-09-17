using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GraphNotificationsTestUwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer appSettings = null;
        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        const string clientId = "7b6e37db-aad2-4952-afbd-24534c777b0d"; // Alternatively "[Enter your client ID, as obtained from the azure portal, e.g. 4e54273c-9fc5-42f4-81b6-60d1b66c9160]"

        const string tenant = "microsoft.onmicrosoft.com"; // Alternatively "[Enter your tenant, as obtained from the azure portal, e.g. kko365.onmicrosoft.com]"
        const string authority = "https://login.microsoftonline.com/" + tenant;

        // To authenticate to the directory Graph, the client needs to know its App ID URI.
        const string resource = "https://graph.microsoft.com";

        // Windows10 universal apps require redirect URI in the format below
        string URI = string.Format("ms-appx-web://Microsoft.AAD.BrokerPlugIn/{0}", WebAuthenticationBroker.GetCurrentApplicationCallbackUri().Host.ToUpper());

        WebAccountProvider wap = null;
        WebAccount userAccount = null;
        string token = null;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            wap = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", authority);
            appSettings = ApplicationData.Current.RoamingSettings;
            WebTokenRequest wtr = new WebTokenRequest(wap, string.Empty, clientId);
            wtr.Properties.Add("resource", resource);

            // Check if there's a record of the last account used with the app
            var userID = appSettings.Values["userID"];
            if (userID != null)
            {
                // Get an account object for the user
                userAccount = await WebAuthenticationCoreManager.FindAccountAsync(wap, (string)userID);
                if (userAccount != null)
                {
                    // Ensure that the saved account works for getting the token we need
                    WebTokenRequestResult wtrr = await WebAuthenticationCoreManager.RequestTokenAsync(wtr, userAccount);
                    if (wtrr.ResponseStatus == WebTokenRequestStatus.Success)
                    {
                        userAccount = wtrr.ResponseData[0].WebAccount;
                        token = wtrr.ResponseData[0].Token;
                    }
                    else
                    {
                        // The saved account could not be used for getitng a token
                        MessageDialog messageDialog = new MessageDialog("We tried to sign you in with the last account you used with this app, but it didn't work out. Please sign in as a different user.");
                        await messageDialog.ShowAsync();
                        // Make sure that the UX is ready for a new sign in
                        UpdateUXonSignOut();
                    }
                }
                else
                {
                    // The WebAccount object is no longer available. Let's attempt a sign in with the saved username
                    wtr.Properties.Add("LoginHint", appSettings.Values["login_hint"].ToString());
                    WebTokenRequestResult wtrr = await WebAuthenticationCoreManager.RequestTokenAsync(wtr);
                    if (wtrr.ResponseStatus == WebTokenRequestStatus.Success)
                    {
                        userAccount = wtrr.ResponseData[0].WebAccount;
                        token = wtrr.ResponseData[0].Token;
                    }
                }
            }
            else
            {
                // There is no recorded user. Let's start a sign in flow without imposing a specific account.                             
                WebTokenRequestResult wtrr = await WebAuthenticationCoreManager.RequestTokenAsync(wtr);
                if (wtrr.ResponseStatus == WebTokenRequestStatus.Success)
                {
                    userAccount = wtrr.ResponseData[0].WebAccount;
                    token = wtrr.ResponseData[0].Token;
                }
            }

            if (userAccount != null) // we succeeded in obtaining a valid user
            {
                // save user ID in local storage
                UpdateUXonSignIn();
            }
            else
            {
                // nothing we tried worked. Ensure that the UX reflects that there is no user currently signed in.
                UpdateUXonSignOut();
                MessageDialog messageDialog = new MessageDialog("We could not sign you in. Please try again.");
                await messageDialog.ShowAsync();
            }
        }

        // update the UX and the app settings to show that a user is signed in
        private void UpdateUXonSignIn()
        {
            appSettings.Values["userID"] = userAccount.Id;
            appSettings.Values["login_hint"] = userAccount.UserName;
            textSignedIn.Text = string.Format("you are signed in as {0} - ", userAccount.UserName);
            textBearer.Text = token;
            btnSignInOut.Content = "Sign in as a different user";
            //btnSearch.IsEnabled = true;
        }

        // update the UX and the app settings to show that no user is signed in at the moment
        private void UpdateUXonSignOut()
        {
            appSettings.Values["userID"] = null;
            appSettings.Values["login_hint"] = null;
            //btnSearch.IsEnabled = false;
            textSignedIn.Text = "You are not signed in. ";
            textBearer.Text = "";
            btnSignInOut.Content = "Sign in";
            //SearchResults.ItemsSource = new List<UserSearchResult>();
        }

        private async void btnSignInOut_Click(object sender, RoutedEventArgs e)
        {
            // prepare a request with 'WebTokenRequestPromptType.ForceAuthentication', 
            // which guarantees that the user will be able to enter an account of their choosing
            // regardless of what accounts are already present on the system
            WebTokenRequest wtr = new WebTokenRequest(wap, string.Empty, clientId, WebTokenRequestPromptType.ForceAuthentication);
            wtr.Properties.Add("resource", resource);
            WebTokenRequestResult wtrr = await WebAuthenticationCoreManager.RequestTokenAsync(wtr);
            if (wtrr.ResponseStatus == WebTokenRequestStatus.Success)
            {
                userAccount = wtrr.ResponseData[0].WebAccount;
                token = wtrr.ResponseData[0].Token;
                UpdateUXonSignIn();
            }
            else
            {
                UpdateUXonSignOut();
                MessageDialog messageDialog = new MessageDialog("We could not sign you in. Please try again.");
                await messageDialog.ShowAsync();
            }
        }
    }
}
