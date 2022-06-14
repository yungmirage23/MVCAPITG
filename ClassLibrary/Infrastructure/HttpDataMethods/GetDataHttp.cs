using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestWebAppl.Models;

namespace ClassLibrary.Models
{
    public class GetDataHttp<T>:IHttpData<T>
    {
        protected static string ApiDomain = "http://webapibokovenka.azurewebsites.net/";
        public  T ResultData { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
        public static async Task<GetDataHttp<T>> CreateAsync(string _apiRoute)
        {
            GetDataHttp<T> x=new GetDataHttp<T>();
            await x.RequestData(_apiRoute);
            return x;
        }
        protected async Task<HttpResponseMessage> RequestData(string _apiRoute)
        {
            
            using (HttpClient _client = new HttpClient())
            {
                _client.BaseAddress = new Uri(ApiDomain);
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage message = await _client.GetAsync(_apiRoute);
                if (message.IsSuccessStatusCode)
                {
                    var Result = message.Content.ReadAsStringAsync().Result;
                    ResultData = JsonConvert.DeserializeObject<T>(Result);
                }
                ResponseMessage = message;
            }
            return ResponseMessage;
        }
    }
}
