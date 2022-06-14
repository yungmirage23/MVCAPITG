using System.Net.Http.Json;
using RestWebAppl.Models;

namespace ClassLibrary.Models
{
    public class DeleteDataHttp<T>:IHttpData<T>
    {
        protected static string ApiDomain = "http://webapibokovenka.azurewebsites.net/";
        public  T ResultData { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
        public static async Task<DeleteDataHttp<T>> CreateAsync(string _apiRoute,T item)
        {
            DeleteDataHttp<T> x=new DeleteDataHttp<T>();
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
                    var deleteTask = client.DeleteAsync(_apiRoute);
                    deleteTask.Wait();
                    ResponseMessage = deleteTask.Result;
                }
            }
            return ResponseMessage;
        }
    }
}
