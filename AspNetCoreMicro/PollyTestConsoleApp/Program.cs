using System;
using System.Threading;
using Polly;
using Polly.Timeout;

namespace PollyTestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //{
            //    ISyncPolicy policy = Policy.Handle<Exception>().RetryForever();

            //    policy.Execute(() =>
            //    {
            //        Console.WriteLine("开始任务！");

            //        if (new Random().Next(10) < 9)
            //        {
            //            throw new ApplicationException("触发异常，重试直到成功！");
            //        }

            //        Console.WriteLine("结束任务！");
            //    });
            //}

            //{

            //    ISyncPolicy policy = Policy.Handle<Exception>().Retry(3);

            //    try
            //    {
            //        policy.Execute(() =>
            //        {
            //            Console.WriteLine("开始任务！");

            //            if (new Random().Next(10) < 9)
            //            {
            //                throw new ApplicationException("触发异常，只试3次！");
            //            }

            //            Console.WriteLine("结束任务！");
            //        });
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}


            //{

            //    ISyncPolicy policy = Policy.Handle<Exception>().WaitAndRetry(new TimeSpan[]{TimeSpan.FromMilliseconds(100) ,TimeSpan.FromMilliseconds(200)});

            //    try
            //    {
            //        policy.Execute(() =>
            //        {
            //            Console.WriteLine("开始任务！");

            //            if (new Random().Next(10) < 9)
            //            {
            //                throw new ApplicationException("触发异常，只试3次！");
            //            }

            //            Console.WriteLine("结束任务！");
            //        });
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //}

            //{
            //    //连续出错6次后熔断5秒，（不会尝试去执行业务代码）
            //    ISyncPolicy policy = Policy.Handle<Exception>().CircuitBreaker(6,TimeSpan.FromSeconds(5));

            //    while (true)
            //    {
            //        Console.WriteLine("开始Execute");
            //        try
            //        {
            //            policy.Execute(() =>
            //            {
            //                Console.WriteLine("开始任务！");
            //                throw new ApplicationException("触发异常");
            //                Console.WriteLine("结束任务！");
            //            });
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine("Execute执行出错："+e.Message);
            //        }
            //        Thread.Sleep(500);
            //    }
            //}


            {
                //出现异常，或者超时
                ISyncPolicy policy = Policy.Handle<Exception>().Fallback(() =>
                {
                    Console.WriteLine("执行出错");
                });
                //悲观会抛异常，乐观不会抛异常
                policy = policy.Wrap(Policy.Timeout(2, TimeoutStrategy.Pessimistic));//悲观/乐观
                
                policy.Execute(() =>
                {
                    Console.WriteLine("开始任务！");
                    Thread.Sleep(5000);
                    Console.WriteLine("结束任务！");
                });
            }

            Console.WriteLine("Hello World!");
        }
    }
}
