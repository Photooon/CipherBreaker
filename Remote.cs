using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace CipherBreaker
{
    class Remote
    {
        private const string encryptUrl = "http://www.mortyw.cn/cipherbreaker/api/encrypt?method={0}&str={1}&key={2}";
        private const string decryptUrl = "http://www.mortyw.cn/cipherbreaker/api/decrypt?method={0}&str={1}&key={2}";
        private const string breakUrl = "http://www.mortyw.cn/cipherbreaker/api/break?method={0}&str={1}";

        private static string SendTo(string Url)           //向目标url发送请求并返回服务端返回的信息
        {
            Console.WriteLine(Url);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            /*POST请求方法*/
            //HttpContent content = new StringContent(JsonConvert.SerializeObject(...), Encoding.UTF8, "application/json");
            //var postTask = client.PostAsync(Url, content);
            //postTask.Wait();

            /*GET请求方法*/
            var getTask = client.GetStringAsync(Url);
            String result = JsonConvert.DeserializeObject<String>(getTask.Result);

            return result;
        }

        public static string Encrypt(string method, string plain, string key)
        {
            return SendTo(String.Format(encryptUrl, method, plain, key));       //这里返回的可能是错误信息（如密钥不正确等），也可能是结果
        }

        public static string Decrypt(string method, string plain, string key)
        {
            return SendTo(String.Format(decryptUrl, method, plain, key));
        }

        public static string Break(string method, string plain, string key)
        {
            return SendTo(String.Format(breakUrl, method, plain));
        }
    }
}