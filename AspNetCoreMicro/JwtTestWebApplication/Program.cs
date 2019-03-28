using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace JwtTestWebApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var payload = new Dictionary<string, object>
            {
                {"UserId","hx123456" },
                {"UserName","admin" }
            };
            var secret = "ASDFQWEFAWDFASDFASGERGSDRGFSDFGSDFGERTGWER";//不要泄露
            IJwtAlgorithm algorithm=new HMACSHA256Algorithm();
            IJsonSerializer serializer=new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder=new JwtBase64UrlEncoder();
            IJwtEncoder encoder=new JwtEncoder(algorithm,serializer,urlEncoder);

            var token = encoder.Encode(payload, secret);
            Console.WriteLine(token);

            try
            {
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
                Console.WriteLine(json);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Token format invalid"+e.Message);
            }
            catch (TokenExpiredException e)
            {
                Console.WriteLine("Token has expired" + e.Message);
            }
            catch (SignatureVerificationException e)
            {
                Console.WriteLine("Token has invalid signature" + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
