using System;
using System.Collections.Generic;
using System.Text;

namespace AspectCoreConsoleApp
{
    public class Person
    {
        [CustomInsterceptor]
        public virtual void Say(string msg)
        {
            Console.WriteLine("service calling ..." + msg);
        }
    }
}
