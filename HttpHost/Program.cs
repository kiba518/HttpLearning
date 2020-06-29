using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpHost
{
    class Program
    {
        static HttpListener httpListener; 
        static volatile bool isRun = true;
        static void Main(string[] args)
        {
            Listener(5180);
        } 
        public static void Listener(int port)
        { 
            //创建HTTP监听
            httpListener = new HttpListener(); 
            //监听的路径
            httpListener.Prefixes.Add($"http://localhost:{port}/");
            httpListener.Prefixes.Add($"http://127.0.0.1:{port}/"); 
            //设置匿名访问
            httpListener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            //开始监听
            httpListener.Start();
             
            while (isRun)
            {
             
                var context = httpListener.GetContext();   //等待传入的请求接受到请求时返回，它将阻塞线程，直到请求到达 
                HttpListenerRequest request = context.Request;   //取得请求的对象
               
                Console.WriteLine($"请求模式：{request.HttpMethod} Accept-Encoding：{request.Headers["Accept-Encoding"]}  ContentEncoding：{ request.ContentEncoding}"); 
                var abcSource = request.QueryString["abc"]; 
                var abc = System.Web.HttpUtility.UrlDecode(abcSource);
                Console.WriteLine($"Get请求abc的值：{abc}"); 


                var reader = new StreamReader(request.InputStream, Encoding.UTF8);
                var msgSource = reader.ReadToEnd();//读取传过来的信息+
                Console.WriteLine($"msgSource：{msgSource}");
                var msg = Uri.UnescapeDataString(msgSource);
                Console.WriteLine($"请求msg：{msg}");
                byte[] ascByte = System.Text.Encoding.UTF8.GetBytes(msg);
                String ascMsg = Encoding.ASCII.GetString(ascByte);

                string responseString = "返回值";
                // 取得回应对象
                HttpListenerResponse response = context.Response;

                // 设置回应头部内容，长度，编码
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "text/plain; charset=utf-8";

                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Cache-Control", "no-cache");

                byte[] buff = Encoding.UTF8.GetBytes(responseString);

                // 输出回应内容
                System.IO.Stream output = response.OutputStream;
                output.Write(buff, 0, buff.Length);
                // 必须关闭输出流
                output.Close();

            }

        }
    }
}
//var list = msg.Split('&');


//string name = "";
//string imgdata = "";
//string type = "";

//                foreach (var item in list)
//                {
//                    var key = item.Split('=');
//                    if (key.Count() >= 2)
//                    {
//                        if (key[0] == "name")
//                        {
//                            name = key[1];
//                        }
//                        if (key[0] == "imgdata")
//                        {
//                            imgdata = item.Replace("imgdata=", "");
//                        }
//                        if (key[0] == "type")
//                        {
//                            type = key[1];
//                        }
//                    }
//                }