using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsWebKit.Model
{
    public class QueryResponse<T>
    {
        public QueryResponse()
        {

        }

        //public string Http_Code { get; set; }
        /// <summary>
        /// 错误代码 0或者空正常 1服务内部异常 2服务安全校验失败，3业务异常
        /// </summary>
        public int status { set; get; }

        public string info { set; get; }

        public T result { set; get; }

    }
}
