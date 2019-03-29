using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServerWebApplication
{
    public class Config
    {
        /// <summary>
        /// 返回应用列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> resources=new List<ApiResource>();
            resources.Add(new ApiResource("SMSWebApplication","短信应用服务"));
            resources.Add(new ApiResource("EmailWebApplication", "邮件应用服务"));
            return resources;
        }

        /// <summary>
        /// 返回账号列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients=new List<Client>();
            clients.Add(new Client()
            {
                ClientId = "davyyang",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    //new Secret("123321",DateTime.Now.AddYears(2))//密钥
                    new Secret("123321".Sha256())//密钥
                },
                AllowedScopes = {"EmailWebApplication", "SMSWebApplication" }//这个账号支持访问哪些应用
            });
            return clients;
        }
        
    }
}
