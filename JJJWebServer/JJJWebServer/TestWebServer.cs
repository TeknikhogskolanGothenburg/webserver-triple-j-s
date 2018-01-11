using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JJJWebServer
{
    public class TestWebServer
    {
        private HttpListener listener = new HttpListener();
        public TestWebServer(string[] prefixes) 
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/"
            if (prefixes == null || prefixes.Length == 0)
            {
                throw new ArgumentException("prefixes");
            }

            foreach (string s in prefixes)
            {
                listener.Prefixes.Add(s);
            }
            listener.Start();

            Console.WriteLine("Listening...");
            
            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                ProcessRequest(context);
            }
            listener.Stop();
        }

        public static void ProcessRequest(HttpListenerContext context)
        {
            string path =@"...\...\...\...\Content\index.html";
            try
            {
                context.Response.StatusCode = (int)HttpStatusCode.Accepted;
                Console.WriteLine(context.Response.StatusCode = (int)HttpStatusCode.Accepted);

                byte[] buffer = File.ReadAllBytes(path);
                context.Response.ContentLength64 = buffer.Length;
                Stream output = context.Response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Processing Problem");
            }

        }

    }
}
