﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace JJJWebServer
{
    class Program
    {
        private static string rootPath = ".../.../.../.../Content/";
        private static string[] DefaultFiles = { "index.html" };

        static void Main(string[] prefixes)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            string[] addresses = { "http://localhost:8080/" };
            // URI prefixes are required,
            // for example "http://localhost:8080/".
            if (addresses == null || addresses.Length == 0)
            {
                throw new ArgumentException("prefixes");
            }

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            foreach (string prefix in addresses)
            {
                listener.Prefixes.Add(prefix);
            }
            listener.Start();
            Console.WriteLine("Listening...");

            Dictionary<string, int> cookieCounter = new Dictionary<string, int>(); //cookievalue, counter

            while (true)
            {
                try
                {
                    //reads in all files from folder
                    var fileList = Directory.GetFiles(rootPath);

                    foreach (var f in fileList)
                    {
                        HttpListenerContext context = listener.GetContext();
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;

                        string cookieValue = string.Empty;
                        if (request.Cookies.Count == 0 || request.Cookies["counter"] == null)
                        {
                            if (cookieCounter.Count == 0)
                            {
                                cookieValue = "1";
                            }
                            else
                            {
                                var max = cookieCounter.Keys.Max(x => int.Parse(x));
                                cookieValue = (max + 1).ToString();
                            }
                            cookieCounter.Add(cookieValue, 1);
                        }
                        else
                        {
                            cookieValue = request.Cookies["counter"].Value;
                            cookieCounter[cookieValue]++;
                        }
                        response.SetCookie(new Cookie("counter", cookieValue));

                        string filename = request.RawUrl;
                        filename = filename.Substring(1);

                        if (string.IsNullOrEmpty(filename))
                        {
                            foreach (string indexFile in DefaultFiles)
                            {
                                if (File.Exists(Path.Combine(rootPath, indexFile)))
                                {
                                    filename = indexFile;
                                    break;
                                }
                            }
                        }
                        if (filename == "Subfolder/")
                        {
                            filename = "SubFolder/index.html";
                        }
                        filename = Path.Combine(rootPath, filename);
                        //runs program if file exists
                        if (File.Exists(filename))
                        {
                            try
                            {
                                Console.WriteLine(filename);
                                Stream input = new FileStream(filename, FileMode.Open);
                                //Adding permanent http response headers
                                context.Response.ContentType = getContentType(Path.GetExtension(filename));
                                Console.WriteLine("Content Type: " + context.Response.ContentType);
                                context.Response.ContentLength64 = input.Length;
                                context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                                context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                                byte[] buffer = new byte[1024 * 32];
                                int nbytes;
                                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                                    context.Response.OutputStream.Write(buffer, 0, nbytes);
                                input.Close();
                                context.Response.OutputStream.Flush();

                                context.Response.StatusCode = (int)HttpStatusCode.OK;
                                Console.WriteLine("Status Code: " + context.Response.StatusCode);
                                context.Response.OutputStream.Close();
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                Console.WriteLine("Status Code: " + context.Response.StatusCode + ":"+ "ex");
                            }
                        }
                        else
                        {
                            
                            if (filename != ".../.../.../.../Content/favicon.ico")
                            {
                                Console.WriteLine(filename);
                                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                Console.WriteLine("Status Code: " + context.Response.StatusCode);
                            }
                        }
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //listener.Close();
        }
        private static string getContentType(string extension)
        {
            switch (extension)
            {
                case ".css": return "text/css";
                case ".gif": return "image/gif";
                case ".htm":
                case ".html": return "text/html";
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                case ".js": return "application/x-javascript";
                case ".png": return "image/png";
                case ".pdf": return "application/pdf";
                case ".txt": return "text/plain";
                default: return "application/octet-stream";
            }
        }
    }
}

