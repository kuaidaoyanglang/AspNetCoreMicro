﻿using System;
using AspectCore.DynamicProxy;

namespace AspectCoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProxyGeneratorBuilder proxyGeneratorBuilder=new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator=proxyGeneratorBuilder.Build())
            {
                Person person = proxyGenerator.CreateClassProxy<Person>();
                person.Say("davy yang");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
