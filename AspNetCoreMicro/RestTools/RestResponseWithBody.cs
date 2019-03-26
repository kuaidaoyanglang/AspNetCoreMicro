using System;
using System.Collections.Generic;
using System.Text;

namespace RestTools
{
    public class RestResponseWithBody<T> : RestResponse
    {
        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
        // (set) Token: 0x06000007 RID: 7 RVA: 0x00002082 File Offset: 0x00000282
        public T Body { get; set; }
    }
}
