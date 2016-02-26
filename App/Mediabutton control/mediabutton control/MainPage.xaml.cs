using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["serverAddress"] != null)
            {
                savedServerAddressTextBlock.Text = localSettings.Values["serverAddress"].ToString();
            }
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        private void http_GET(string address)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string serverAddress = localSettings.Values["serverAddress"].ToString();
            if (serverAddress != null)
            {
                string url = "http://" + serverAddress + address;
                Debug.WriteLine(url);
                GetFullResponse(url);
            }
            else
            {
                ErrorTextBox.Text = "You forgot to input the server IP";
            }

        }
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            http_GET("/previoussong");
        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            http_GET("/nextsong");
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            http_GET("/playpause");


        }
        private void volumeupButton_Click(object sender, RoutedEventArgs e)
        {
            http_GET("/volumeup");
        }
        private void volumedownButton_Click(object sender, RoutedEventArgs e)
        {
            http_GET("/volumedown");
        }
        private void muteButton_Click(object sender, RoutedEventArgs e)
        {

            http_GET("/volumemute");

        }
        public async void GetFullResponse(String url)
        {
            try
            {
                //if you hit the same urls repeatedly the program will serve you the same thing and not access the server so we add unix timestamp to the url to make it unique each time
                int unixTimestamp = (int)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                //Create Client 
                var client = new HttpClient();

                //Define URL. Replace current URL with your URL
                //Current URL is not a valid one

                //adding unix timestamp to url query strings to make it unique. I explained this in earlier comment
                var uri = new Uri(url + "?time=" + unixTimestamp.ToString());

                //Call. Get response by Async
                var Response = await client.GetAsync(uri);

                //Result & Code
                var statusCode = Response.StatusCode;

                //If Response is not Http 200 
                //then EnsureSuccessStatusCode will throw an exception
                Response.EnsureSuccessStatusCode();

                //Read the content of the response.
                //In here expected response is a string.
                //Accroding to Response you can change the Reading method.
                //like ReadAsStreamAsync etc..
                var ResponseText = await Response.Content.ReadAsStringAsync();
                Debug.WriteLine(ResponseText);
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                ErrorTextBox.Text = "Error: Make sure your server is running and the ip you've set is correct";
            }
        }
        private void AddServerPToStorage(string ip)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["serverAddress"] = serverIPTextBox.Text;

        }
        private void GotFocusAction(object sender, RoutedEventArgs e)
        {
            serverIPTextBox.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            serverIPTextBox.BorderBrush = new SolidColorBrush(Windows.UI.Colors.White);
            serverIPTextBox.Background = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void LostFocusAction(object sender,RoutedEventArgs e)
        {
            serverIPTextBox.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
            serverIPTextBox.BorderBrush = new SolidColorBrush(Windows.UI.Colors.White);
            serverIPTextBox.Background = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["serverAddress"] = serverIPTextBox.Text;
            savedServerAddressTextBlock.Text = localSettings.Values["serverAddress"].ToString();
        }
    }
}

