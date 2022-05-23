using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public interface IHttpData<T>
    {
        protected static string ApiDomain = "http://localhost:5263/";
        public T ResultData { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }

    }
}
