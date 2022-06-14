using System.Net.Http.Json;
using RestWebAppl.Models;

namespace ClassLibrary.Models
{
    public class PostDataHttp<T>:IHttpData<T>
    {
        protected static string ApiDomain = "http://webapibokovenka.azurewebsites.net/";
        public  T ResultData { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
        public static async Task<PostDataHttp<T>> CreateAsync(string _apiRoute,T item)
        {
            PostDataHttp<T> x=new PostDataHttp<T>();
            await x.RequestData(_apiRoute,item);
            return x;
        }
        protected async Task<HttpResponseMessage> RequestData(string _apiRoute,T item)
        {
            
            using (HttpClient _client = new HttpClient())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ApiDomain);
                    var postTask = await client.PostAsJsonAsync<T>(_apiRoute, item);
                    ResponseMessage = postTask;
                }
            }
            return ResponseMessage;
        }
        public static async Task<PostDataHttp<T>> CreateAsync(string _apiRoute, T item,string _ApiDomain)
        {
            PostDataHttp<T> x = new PostDataHttp<T>();
            await x.RequestData(_apiRoute, item, _ApiDomain);
            return x;
        }
        protected  async Task<HttpResponseMessage> RequestData(string _apiRoute, T item, string _ApiDomain)
        {

            using (HttpClient _client = new HttpClient())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_ApiDomain);
                    var postTask = await client.PostAsJsonAsync<T>(_apiRoute, item);
                  
                    ResponseMessage = postTask;
                }
            }
            return ResponseMessage;
        }
    }
}
