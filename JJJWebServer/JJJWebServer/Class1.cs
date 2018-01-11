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
            // Note: The GetContext method blocks while waiting for a request. 
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            // Construct a response.
            //string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            //byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            //// Get a response stream and write the response to it.
            //response.ContentLength64 = buffer.Length;
            //Stream output = response.OutputStream;
            //output.Write(buffer, 0, buffer.Length);
            //// You must close the output stream.
            //output.Close();

            while (listener.IsListening)
            {
                HttpListenerContext contentType = listener.GetContext();
                ProcessRequest(context);
            }
            listener.Stop();
        }

        public static void ProcessRequest(HttpListenerContext contentType)
        {
            string path =@"...\...\...\Content";
            try
            {
                contentType.Response.StatusCode = (int)HttpStatusCode.Accepted;
                Console.WriteLine(contentType.Response.StatusCode = (int)HttpStatusCode.Accepted);
                var receivingFile = File.ReadAllBytes(path);
                Console.WriteLine(receivingFile);
                contentType.Response.ContentLength64 = receivingFile.Length;
                Stream output = contentType.Response.OutputStream;
                output.Write(receivingFile, 0, receivingFile.Length);
            }
            catch (Exception e)
            {

                Console.WriteLine("Processing Problem");
            }

        }

    }
}
