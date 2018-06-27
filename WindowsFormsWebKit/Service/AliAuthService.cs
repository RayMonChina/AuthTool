using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsWebKit.Model;

namespace WindowsFormsWebKit.Service
{
    public class AliAuthService
    {
        public List<AliAccountDto> GetAccountData() {
            List<AliAccountDto> retList=new List<AliAccountDto>();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var accountFilePath = Path.Combine(baseDir, "data/aliAccount.json");
            if (!File.Exists(accountFilePath)) {
                return retList;
            }
            using (var sr = new StreamReader(accountFilePath,Encoding.UTF8)) {
                var fileContent = sr.ReadToEnd();
                retList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AliAccountDto>>(fileContent);
            }
            return retList;
        }

        public string GetSignUrl(int userId) {
            if (userId <= 0)
                return string.Empty;
            var reqUrl = string.Format("https://ccapi.gaodun.com/api/ali/auth?userId={0}",userId);
            var result = RestClient.Get<QueryResponse<string>>(reqUrl, null, 5);
            return result.result;
        }
    }
}
