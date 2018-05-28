using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsWebKit.Service
{
    public class RestClient
    {
        private RestClient() { }
        private int timeSecond = 10;//单位（s）

        public static K Get<K>(string requestUrl, NameValueCollection headerParameters = null, int timeOut = 20)
        {
            RestClient client = new RestClient();
            if (timeOut > 0) {
                client.timeSecond = timeOut;
            }
            return client.DoInvoke<object, K>(requestUrl, "Get", null, headerParameters);
        }

        public static K Post<K>(string requestUrl, object item, NameValueCollection headerParameters = null, int timeOut = 10)
        {
            RestClient client = new RestClient();
            if (timeOut > 0)
            {
                client.timeSecond = timeOut;
            }
            return client.DoInvoke<object, K>(requestUrl, "Post", item, headerParameters);
        }

        public static K Put<K>(string requestUrl, object item, NameValueCollection headerParameters = null, int timeOut = 10)
        {
            RestClient client = new RestClient();
            if (timeOut > 0)
            {
                client.timeSecond = timeOut;
            }
            return client.DoInvoke<object, K>(requestUrl, "Put", item, headerParameters);
        }

        private K DoInvoke<T, K>(string requestUrl, string httpMethod, T item, NameValueCollection headerParameters)
        {
            int millisecondsTimeout =timeSecond*1000;

            if (requestUrl.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);//兼容https证书
            }

            HttpWebRequest request = this.CreateHttpWebRequest(requestUrl, httpMethod, millisecondsTimeout);
            string content = string.Empty;
            if (item != null && !string.Equals(httpMethod, "Get", StringComparison.InvariantCultureIgnoreCase)) {
                if (typeof(string) == item.GetType())
                {
                    content =Convert.ToString(item);
                }
                else {
                    content = Newtonsoft.Json.JsonConvert.SerializeObject(item);
                }
            }
            if (headerParameters != null && headerParameters.Count > 0)
            {
                foreach (string headerItem in headerParameters)
                {
                    request.Headers.Add(headerItem, headerParameters[headerItem]);
                }
            }

            if (content != null && content.Length > 0)
            {
                byte[] data = Encoding.UTF8.GetBytes(content);
                request.ContentLength = data.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return this.ReadResponse<K>(response, false, requestUrl);
            }
        }

        private HttpWebRequest CreateHttpWebRequest(string requestUrl, string httpMethod, int millisecondsTimeout)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = httpMethod.ToUpper();

            request.ContentType = "application/json;charset=UTF-8";
            request.Accept = "application/json;charset=UTF-8";
            request.Timeout = millisecondsTimeout + 10;

            return request;
        }

        private T ReadResponse<T>(HttpWebResponse response, bool isVoidResult, string requestUrl)
        {
            if ((response.StatusCode == HttpStatusCode.OK||response.StatusCode==HttpStatusCode.Created)
                && !isVoidResult)
            {
                string responseContent = null;
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, System.Text.Encoding.UTF8))
                    {
                        responseContent = streamReader.ReadToEnd();
                       
                        streamReader.Close();
                    }

                    responseStream.Flush();
                    responseStream.Close();
                }

                if (typeof(T) == typeof(string))
                {
                    object value = responseContent;
                    return (T)value;
                }

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseContent);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                string message = string.Format("{0}-{1}, url:{2}",
                    (int)response.StatusCode,
                    response.StatusDescription,
                    requestUrl);

                throw new Exception(message);
            }

            return default(T);
        }
    }
}
