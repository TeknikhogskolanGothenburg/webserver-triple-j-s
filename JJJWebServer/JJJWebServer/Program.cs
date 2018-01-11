using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JJJWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
        string[] prefixes = {"http://localhost:8080/"};
        TestWebServer ws = new TestWebServer(prefixes);
        }
    }
}
