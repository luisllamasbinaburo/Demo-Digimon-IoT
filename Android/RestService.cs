using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio.Channels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NFCAndroid
{
    public class RestService
    {
        private string _url = String.Empty;

        public RestService()
        {
        }

        public void Initialize(string url)
        {
            _url = url;
        }

        public void PostItem(string data)
        {
            try
            {
                var client = new RestClient(_url);
                var request = new RestRequest("item", Method.Post);               
                request.AddBody(JsonSerializer.Serialize(new { data }));

                var response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
            catch (Exception ex)
            {
                //do nothing
            }         
        }
    }
}