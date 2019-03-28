using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenServerWebApplication
{
    public class APIReault<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
