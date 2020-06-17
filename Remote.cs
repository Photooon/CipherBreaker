using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
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

        private static string getMethodStr(SchemeType st)
        {
            string methodStr;

            switch (st)
            {
                case SchemeType.Caesar:
                    methodStr = "caesar";
                    break;
                case SchemeType.Affine:
                    methodStr = "affine";
                    break;
                case SchemeType.Columnar:
                    methodStr = "columnar";
                    break;
                case SchemeType.RailFence:
                    methodStr = "railfence";
                    break;
                case SchemeType.Substitution:
                    methodStr = "substitution";
                    break;
                case SchemeType.Transposition:
                    methodStr = "transposition";
                    break;
                case SchemeType.Vigenere:
                    methodStr = "vigenere";
                    break;
                default:
                    methodStr = "";
                    break;
            }

            return methodStr;
        }

        public static string Encrypt(SchemeType method, string plain, string key)
        {
            return SendTo(String.Format(encryptUrl, getMethodStr(method), plain, key));       //这里返回的可能是错误信息（如密钥不正确等），也可能是结果
        }

        public static string Decrypt(SchemeType method, string plain, string key)
        {
            return SendTo(String.Format(decryptUrl, getMethodStr(method), plain, key));
        }

        public static string Break(SchemeType method, string plain, string key)
        {
            return SendTo(String.Format(breakUrl, getMethodStr(method), plain));
        }
    }
}