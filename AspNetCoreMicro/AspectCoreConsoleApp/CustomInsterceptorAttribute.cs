using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace AspectCoreConsoleApp
{
    public class CustomInsterceptorAttribute : AbstractInterceptorAttribute
    {
        //每个拦截方法中执行
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine("Before service call");
                await next(context); //执行被拦截的方法
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            finally
            {
                Console.WriteLine("After service call");
            }
        }
    }
}
