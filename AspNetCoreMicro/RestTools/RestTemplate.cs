using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;

namespace RestTools
{
    // Token: 0x02000004 RID: 4
    public class RestTemplate
    {
        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000009 RID: 9 RVA: 0x00002093 File Offset: 0x00000293
        // (set) Token: 0x0600000A RID: 10 RVA: 0x0000209B File Offset: 0x0000029B
        public string ConsulServerUrl { get; set; } = "http://127.0.0.1:8500";

        // Token: 0x0600000B RID: 11 RVA: 0x000020A4 File Offset: 0x000002A4
        public RestTemplate(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x000020C0 File Offset: 0x000002C0
        private async Task<string> ResolveRootUrlAsync(string serviceName)
        {
            string result;
            using (ConsulClient consulClient = new ConsulClient(delegate (ConsulClientConfiguration c)
            {
                c.Address = new Uri(this.ConsulServerUrl);
            }))
            {
                IEnumerable<AgentService> source = from s in (await consulClient.Agent.Services(default(CancellationToken))).Response.Values
                                                   where s.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)
                                                   select s;
                if (!source.Any<AgentService>())
                {
                    throw new ArgumentException(string.Format("找不到服务{0}的任何实例", serviceName));
                }
                AgentService agentService = source.ElementAt(Environment.TickCount % source.Count<AgentService>());
                result = string.Format("{0}:{1}", agentService.Address, agentService.Port);
            }
            return result;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002110 File Offset: 0x00000310
        private async Task<string> ResolveUrlAsync(string url)
        {
            Uri uri = new Uri(url);
            string host = uri.Host;
            string str = await this.ResolveRootUrlAsync(host);
            return uri.Scheme + "://" + str + uri.PathAndQuery;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002160 File Offset: 0x00000360
        public async Task<RestResponseWithBody<T>> GetForEntityAsync<T>(string url, HttpRequestHeaders requestHeaders = null)
        {
            RestResponseWithBody<T> result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Get;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                result = await this.SendForEntityAsync<T>(requestMsg);
            }
            return result;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000021B8 File Offset: 0x000003B8
        public async Task<RestResponseWithBody<T>> PostForEntityAsync<T>(string url, object body = null, HttpRequestHeaders requestHeaders = null)
        {
            RestResponseWithBody<T> result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Post;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                requestMsg.Content = new StringContent(JsonConvert.SerializeObject(body));
                requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result = await this.SendForEntityAsync<T>(requestMsg);
            }
            return result;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002218 File Offset: 0x00000418
        public async Task<RestResponse> PostAsync(string url, object body = null, HttpRequestHeaders requestHeaders = null)
        {
            RestResponse result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Post;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                requestMsg.Content = new StringContent(JsonConvert.SerializeObject(body));
                requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result = await this.SendAsync(requestMsg);
            }
            return result;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002278 File Offset: 0x00000478
        public async Task<RestResponseWithBody<T>> PutForEntityAsync<T>(string url, object body = null, HttpRequestHeaders requestHeaders = null)
        {
            RestResponseWithBody<T> result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Put;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                requestMsg.Content = new StringContent(JsonConvert.SerializeObject(body));
                requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result = await this.SendForEntityAsync<T>(requestMsg);
            }
            return result;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000022D8 File Offset: 0x000004D8
        public async Task<RestResponse> PutAsync(string url, object body = null, HttpRequestHeaders requestHeaders = null)
        {
            RestResponse result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Put;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                requestMsg.Content = new StringContent(JsonConvert.SerializeObject(body));
                requestMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                result = await this.SendAsync(requestMsg);
            }
            return result;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002338 File Offset: 0x00000538
        public async Task<RestResponseWithBody<T>> DeleteForEntityAsync<T>(string url, HttpRequestHeaders requestHeaders = null)
        {
            RestResponseWithBody<T> result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Delete;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                result = await this.SendForEntityAsync<T>(requestMsg);
            }
            return result;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002390 File Offset: 0x00000590
        public async Task<RestResponse> DeleteAsync(string url, HttpRequestHeaders requestHeaders = null)
        {
            RestResponse result;
            using (HttpRequestMessage requestMsg = new HttpRequestMessage())
            {
                if (requestHeaders != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in requestHeaders)
                    {
                        requestMsg.Headers.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                requestMsg.Method = HttpMethod.Delete;
                HttpRequestMessage httpRequestMessage = requestMsg;
                string uriString = await this.ResolveUrlAsync(url);
                httpRequestMessage.RequestUri = new Uri(uriString);
                httpRequestMessage = null;
                result = await this.SendAsync(requestMsg);
            }
            return result;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000023E8 File Offset: 0x000005E8
        public async Task<RestResponseWithBody<T>> SendForEntityAsync<T>(HttpRequestMessage requestMsg)
        {
            HttpResponseMessage httpResponseMessage = await this.httpClient.SendAsync(requestMsg);
            RestResponseWithBody<T> respEntity = new RestResponseWithBody<T>();
            respEntity.StatusCode = httpResponseMessage.StatusCode;
            respEntity.Headers = httpResponseMessage.Headers;
            string text = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(text))
            {
                respEntity.Body = JsonConvert.DeserializeObject<T>(text);
            }
            return respEntity;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002438 File Offset: 0x00000638
        public async Task<RestResponse> SendAsync(HttpRequestMessage requestMsg)
        {
            HttpResponseMessage httpResponseMessage = await this.httpClient.SendAsync(requestMsg);
            return new RestResponse
            {
                StatusCode = httpResponseMessage.StatusCode,
                Headers = httpResponseMessage.Headers
            };
        }

        // Token: 0x04000005 RID: 5
        private HttpClient httpClient;
    }
}
