using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace AspNetCoreMicro
{
    public class Tools
    {
        /// <summary>
        /// 获取一个随机可用的IP
        /// </summary>
        /// <param name="minPort"></param>
        /// <param name="maxPort"></param>
        /// <returns></returns>
        public static int GetRandAvailablePort(int minPort=1024,int maxPort=65535)
        {
            Random rand=new Random();
            while (true)
            {
                int port = rand.Next(minPort, maxPort);
                if (!IsPortUsed(port))
                {
                    return port;
                }
            }
        }

        /// <summary>
        /// 判断端口是否在使用中
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool IsPortUsed(int port)
        {
            IPGlobalProperties ipGlobalProperties=IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();
            if (ipsTCP.Any(p=>p.Port==port))
            {
                return true;
            }

            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();
            if (ipsTCP.Any(p => p.Port == port))
            {
                return true;
            }

            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            if (tcpConnInfoArray.Any(conn=>conn.LocalEndPoint.Port==port))
            {
                return true;
            }
            return false;
        }
    }
}
