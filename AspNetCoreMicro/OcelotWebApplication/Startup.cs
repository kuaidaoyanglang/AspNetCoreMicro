using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Consul;
using IdentityServer4.AccessTokenValidation;
using JWT;
using Ocelot.Provider.Consul;
using JWT.Algorithms;
using JWT.Serializers;
using Newtonsoft.Json;

namespace OcelotWebApplication
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
            services.AddAuthentication()
                //对配置文件中使用了ChatKey配置了AuthenticationProviderKey=ChatKey
                //的路由规则使用如下的验证方式
                .AddIdentityServerAuthentication("ChatKey", option =>
                {
                    //IdentityService认证服务器地址
                    option.Authority = "http://127.0.0.1:9888";
                    option.ApiName = "chatapi";//要连接的应用名称
                    option.RequireHttpsMetadata = false;
                    option.SupportedTokens = SupportedTokens.Both;
                    option.ApiSecret = "123321";//密钥
                });
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOcelot(Configuration).AddConsul();
            //services.addcon
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var configuration = new OcelotPipelineConfiguration()
            {
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    if (!ctx.HttpContext.Request.Path.Value.ToUpper().StartsWith("/Token/Auth".ToUpper()))
                    {
                        string token = ctx.HttpContext.Request.Headers["token"].FirstOrDefault();
                        ctx.HttpContext.Request.Headers.Add("xxtoken", token?.ToUpper() ?? "What fuck are you doing?");
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            ctx.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            using (StreamWriter writer=new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("token required");
                            }
                            return;
                        }
                        var secret = "ASDFQWEFAWDFASDFASGERGSDRGFSDFGSDFGERTGWER";//不要泄露
                        IJsonSerializer serializer = new JsonNetSerializer();
                        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                        IDateTimeProvider provider = new UtcDateTimeProvider();
                        IJwtValidator validator = new JwtValidator(serializer, provider);
                        IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                        try
                        {
                            var json = decoder.Decode(token, secret, verify: true);

                            Console.WriteLine(json);

                            dynamic payload = JsonConvert.DeserializeObject<dynamic>(json);

                            string userName = payload.UserName;
                            ctx.HttpContext.Request.Headers.Add("X-UserName", userName);
                        }
                        catch (FormatException)
                        {
                            ctx.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (StreamWriter writer = new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("Token format invalid");
                            }
                            return;
                        }
                        catch (TokenExpiredException)
                        {
                            ctx.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (StreamWriter writer = new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("token has expired");
                            }
                            return;
                        }

                        catch (SignatureVerificationException)
                        {
                            ctx.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (StreamWriter writer = new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("token has invalid signature");
                            }
                            return;
                        }
                    }
                    
                    await next.Invoke();
                }
            };

            app.UseOcelot(configuration).Wait();
            //app.UseMvc();
        }
    }
}
