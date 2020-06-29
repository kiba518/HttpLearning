using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpRequestWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            HttpWebHelper.Post("http://127.0.0.1:5180/", "abc=post 阿 呀", (ret) => {

            });
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            HttpWebHelper.Get("http://127.0.0.1:5180/?abc=post 阿 呀", (ret) => {

            });
        }
    }
    public class HttpWebHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param">string param = "type=" + type + "&username=" + username + "&password=" + password + "&email=" + email;</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static void Post(string url, string param, Action<string> callback)
        {
            new Task(() =>
            {
                try
                {
                    //转换输入参数的编码类型，获取bytep[]数组 
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);
                    //初始化新的webRequst
                    //1． 创建httpWebRequest对象
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    //2． 初始化HttpWebRequest对象
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = byteArray.Length;
                    
                    //3． 附加要POST给服务器的数据到HttpWebRequest对象(附加POST数据的过程比较特殊，它并没有提供一个属性给用户存取，需要写入HttpWebRequest对象提供的一个stream里面。)
                    Stream newStream = webRequest.GetRequestStream();//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Close();
                    //4． 读取服务器的返回信息
                    using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string ret = stream.ReadToEnd();
                            callback(ret);
                        }
                    }
                }
                catch (Exception ex)
                {
                    callback("异常：" + ex.Message);
                }
            }).Start();
        }

        public static void Get(string url, Action<string> callback)
        {
            new Task(() =>
            {
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "GET"; 

                    using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        using (StreamReader stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string ret = stream.ReadToEnd();
                            callback(ret);
                        }
                    }
                }
                catch (Exception ex)
                {
                    callback("异常：" + ex.Message);
                }
            }).Start();

        }

       
    }
}
