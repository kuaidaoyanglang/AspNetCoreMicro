using System;
using System.Collections.Generic;
using Consul;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using RestTools;

namespace QueryConsul1
{
    class Program
    {

        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("http://127.0.0.1:8500") };
            RestTemplate rest = new RestTemplate(httpClient);
            RestResponseWithBody<List<string>> resp =
                rest.GetForEntityAsync<List<string>>("http://AspNetCoreMicro/api/values").Result;
            Console.WriteLine(resp.StatusCode);
            Console.WriteLine(string.Join(",",resp.Body));
            Console.WriteLine("Hello World!");
        }

        //static void Main(string[] args)
        //{
        //    using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
        //    {
        //        var services = consulClient.Agent.Services().Result.Response;
        //        // var agentServices = services.Where(s =>
        //        // s.Value.Service.Equals(serviceName, StringComparison.CurrentCultureIgnoreCase));

        //        foreach (var kv in services)
        //        {
        //            Console.WriteLine($"key={kv.Key},{kv.Value.Address},{kv.Value.ID}");
        //        }

        //        var agentServices = services.Where(s =>
        //        s.Value.Service.Equals("AspNetCoreMicro", StringComparison.CurrentCultureIgnoreCase))
        //            .Select(m=>m.Value);
        //        var agentService = agentServices.ElementAt(Environment.TickCount % agentServices.Count());

        //       Console.WriteLine($"{agentService.Address},{agentService.ID}");

        //    }
        //    Console.WriteLine("Hello World!");
        //}
    }
}
