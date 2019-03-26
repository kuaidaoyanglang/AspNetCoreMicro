using System;
using System.Net;
using System.Net.Http.Headers;

namespace RestTools
{
    public class RestResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseHeaders Headers { get; set; }//响应的报文头
    }
}
