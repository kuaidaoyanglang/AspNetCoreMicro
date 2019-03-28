using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TokenServerWebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            string ip = "127.0.0.1";
            int port = 8888;
            //向Consul注册服务 
            var client = new ConsulClient(ConfigurationOverview);

            var result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = "TokenServerWebApplication" + Guid.NewGuid(),//服务编号，不能重复。用Guid最简单
                Name = "TokenServerWebApplication",//服务的名字
                Address = ip,//我的IP地址(可以被其他应用访问的接口，本地测试可以用127.0.0.1，实际机房中一定要写自己的内网IP地址)
                Port = port,
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务停止多久后反注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = $"http://{ip}:{port}/api/health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
                //应用停止的时候反注册
            });

        }

        private void ConfigurationOverview(ConsulClientConfiguration obj)
        {
            obj.Address = new Uri("http://127.0.0.1:8500");
            obj.Datacenter = "dc1";
        }
    }
}
