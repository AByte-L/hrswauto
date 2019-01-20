using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ProgramMonitor.Service
{
    public class WeChat
    {
        private readonly string _url = null;
        private readonly string _corpid = null;
        private readonly string _secret = null;
        public WeChat()
        {
            _url = "https://qyapi.weixin.qq.com/cgi-bin";
            _corpid = "CorpID是企业号的标识，每个企业号拥有一个唯一的CorpID";
            _secret = "secret是管理组凭证密钥，系统管理员在企业号管理后台创建管理组时，企业号后台为该管理组分配一个唯一的secret";
        }


        public string GetToken(string url_prefix = "/")
        {
            string urlParams = string.Format("corpid={0}&corpsecret={1}", HttpUtility.UrlEncodeUnicode(_corpid), HttpUtility.UrlEncodeUnicode(_secret));
            string url = _url + url_prefix + "gettoken?" + urlParams;
            string result = HttpGet(url);
            var content = JObject.Parse(result);
            return content["access_token"].ToString();
        }

        public JObject PostData(dynamic data, string url_prefix = "/")
        {
            string dataStr = JsonConvert.SerializeObject(data);
            string url = _url + url_prefix + "message/send?access_token=" + GetToken();
            string result = HttpPost(url, dataStr);
            return JObject.Parse(result);
        }

        public JObject SendMessage(string touser, string message)
        {
            var data = new { touser = touser, toparty = "1", msgtype = "text", agentid = "2", text = new { content = message }, safe = "0" };
            var jResult = PostData(data);
            return jResult;
        }


        private string HttpPost(string Url, string postDataStr)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] data = Encoding.UTF8.GetBytes(postDataStr);
            request.ContentLength = data.Length;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(data, 0, data.Length);
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string HttpGet(string Url, string urlParams = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (string.IsNullOrEmpty(urlParams) ? "" : "?") + urlParams);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
