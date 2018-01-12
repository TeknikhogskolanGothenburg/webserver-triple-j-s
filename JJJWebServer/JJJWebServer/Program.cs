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
        private static Dictionary<string, string> extensions = new Dictionary<string, string>()
        {
            { "htm", "text/html" },
            { "html", "text/html" },
            { "xml", "text/xml" },
            { "css", "text/css" },
            { "gif", "image/gif" },
            { "jpg", "image/jpg" },
            { "jpeg", "image/jpeg" },
            { "ico", "image/x-icon" }
        };

        private static string rootPath = ".../.../.../.../Content/";
        private static string[] DefaultFiles = { "index.html" };

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
            {
                throw new ArgumentException("prefixes");
            }

            // Create a listener.
            HttpListener listener = new HttpListener();
            // Add the prefixes.
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Listening...");

            while (true)
            {
                try
                {
                    //reads in all files from folder
                    byte[] fileList = File.ReadAllBytes(rootPath + "index.html");

                    foreach (byte b in fileList)
                    {
                        HttpListenerContext context = listener.GetContext();
                        HttpListenerRequest request = context.Request;
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
                        if(filename == "Subfolder/")
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
                                context.Response.ContentType = extensions.TryGetValue(Path.GetExtension(filename), out string mime)
                                    ? mime
                                    : "text/html";
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
                                context.Response.OutputStream.Close();
                            }
                            catch (Exception ex)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            listener.Close();
        }
    }
}

