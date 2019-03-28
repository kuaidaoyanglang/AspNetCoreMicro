using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;

namespace TokenServerWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<APIReault<string>> Get(string userName,string password)
        {
            APIReault<string> result=new APIReault<string>();
            if (userName=="davy yang" && password=="123456")
            {
                var payload=new Dictionary<string,object>()
                {
                    {"UserName",userName },
                    {"UserId",6666 }
                };

                var secret = "ASDFQWEFAWDFASDFASGERGSDRGFSDFGSDFGERTGWER";//不要泄露
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

                var token = encoder.Encode(payload, secret);

                result.Code = 0;
                result.Data = token;

            }

            else
            {
                result.Code = -1;
                result.Message = "username or password error";
            }

            Console.WriteLine("发送短信");
            return result;
        }

    }
}
