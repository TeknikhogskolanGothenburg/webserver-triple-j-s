using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JJJWebServer
{
    class Program
    {
        static void Main(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            // for example "http://localhost:8080/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add("http://localhost:8080/");
            
            listener.Start();
            Console.WriteLine("Listening...");
            
            while (listener.IsListening)
            {
                string path = @"...\...\...\...\Content";
                // Note: The GetContext method blocks while waiting for a request. 
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                //GetContentTypeStuff(context);
                HttpListenerResponse response = context.Response;
                // Construct a response.
                byte[] buffer = File.ReadAllBytes(path);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }
            
            listener.Stop();
        }

        public static void GetContentTypeStuff(HttpListenerContext context)
        {
            if (Path.GetExtension(context.Request.RawUrl) == ".jpg")
            {
                context.Response.ContentType = "image/jpeg";
            }
            else if (Path.GetExtension(context.Request.RawUrl) == ".gif")
            {
                context.Response.ContentType = "image/gif";
            }
            else if (Path.GetExtension(context.Request.RawUrl) == ".css")
            {
                context.Response.ContentType = "text/css";
            }
            else if (Path.GetExtension(context.Request.RawUrl) == ".htm")
            {
                context.Response.ContentType = "text/html";
            }
            else if (Path.GetExtension(context.Request.RawUrl) == ".html")
            {
                context.Response.ContentType = "text/html";
            }
            else if (Path.GetExtension(context.Request.RawUrl) == ".ico")
            {
                context.Response.ContentType = "image/x-icon";
            }
            else if (Path.GetExtension(context.Request.RawUrl) == ".js")
            {
                context.Response.ContentType = "application/x-javascript";
            }
        }
        
    }
}
