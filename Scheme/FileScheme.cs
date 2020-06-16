using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CipherBreaker
{
    /// <summary>
    /// 工具类：文件与二进制流间的转换
    /// </summary>
    class FileScheme
    {
        /// <summary>
        /// 将文件转换为byte数组并加密
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换并加密后的byte数组</returns>
        public void File2Bytes(string path, string savepath)
        {
            try
            {
                FileInfo fi = new FileInfo(path);       //用来操作文件
                byte[] buff = new byte[fi.Length];      //定义字节数组长度

                FileStream fs = fi.OpenRead();
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                fs.Close();


                string strTemp;
                strTemp = System.Text.Encoding.Default.GetString(buff);

                Scheme railFence = Scheme.ChooseScheme(plain: strTemp, cipher: null, key: "4");
                (var cipher, _) = railFence.Encode();
                //RailFence railFence = new RailFence(plain: strTemp, key: "4");
                //(var cipher, _) = railFence.Encode();
                byte[] cipherByte = System.Text.Encoding.Default.GetBytes(cipher);


                if (File.Exists(savepath))
                {
                    File.Delete(savepath);
                }

                FileStream decodingSave = new FileStream(savepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(decodingSave);
                bw.Write(cipherByte, 0, cipherByte.Length);
                bw.Close();
                decodingSave.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception:"+e.ToString());
            }
        }

            /// <summary>
            /// 将byte数组解密并转换为文件并保存到指定地址
            /// </summary>
            /// <param name="buff">要解密的byte数组</param>
            /// <param name="savepath">保存地址</param>
        public void Bytes2File(string path, string savepath)
        {
            try
            {
                FileInfo fi = new FileInfo(path);       //用来操作文件
                byte[] buff = new byte[fi.Length];      //定义字节数组长度

                FileStream fs = fi.OpenRead();
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                fs.Close();


                string strTemp;
                strTemp = System.Text.Encoding.Default.GetString(buff);
                Scheme railFence = Scheme.ChooseScheme(plain: null, cipher: strTemp, key: "4");
                (var plain, _) = railFence.Decode();
                //RailFence railFence = new RailFence(cipher: strTemp, key: "4");
                //(var plain, _) = railFence.Decode();
                byte[] cipherByte = System.Text.Encoding.Default.GetBytes(plain);



                if (File.Exists(savepath))
                {
                    File.Delete(savepath);
                }

                FileStream decodersave = new FileStream(savepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(decodersave);
                bw.Write(cipherByte, 0, cipherByte.Length);
                bw.Close();
                decodersave.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e.ToString());
            }
        }
    }    
}
