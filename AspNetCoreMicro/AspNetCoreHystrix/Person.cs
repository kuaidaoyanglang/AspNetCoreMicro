using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreHystrix
{
    public class Person
    {
        [CustomInsterceptor]
        public virtual string Say(string msg)
        {
            Console.WriteLine("service calling ..." + msg);
            return msg;
        }
    }
}
