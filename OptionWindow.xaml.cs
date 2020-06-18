using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;

namespace CipherBreaker
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        private Settings settings;
        Dictionary<string, SchemeType> defaultDict;

        public OptionWindow()
        {
            InitializeComponent();
        }

        public class XMLSaveAndRead
        {
            //将byte数据流转化为string
            private string UTF8ByteArrayToString(byte[] characters)
            {
                UTF8Encoding encoding = new UTF8Encoding();
                string constructedString = encoding.GetString(characters);
                return (constructedString);
            }
            //将string转化为byte数据流
            private byte[] StringToUTF8ByteArray(string pXmlString)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(pXmlString);
                return byteArray;
            }

            // 序列化XML数据
            public string SerializeObject<T>(object pObject)
            {
                string XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            // 反序列化XML数据
            public object DeserializeObject<T>(string pXmlizedString)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }

            // 创建XML文件，若文件存在则找到该文件,参数1为xml路径，参数2为需要写入的信息
            public void CreateXML(string xmlPath, string xmlData)
            {
                StreamWriter writer;
                FileInfo xml = new FileInfo(xmlPath);
                if (!xml.Exists)
                {
                    writer = xml.CreateText();
                }
                else
                {
                    xml.Delete();
                    writer = xml.CreateText();
                }
                writer.Write(xmlData);
                writer.Close();
            }
            //读取XML文件 参数1为xml路径 返回读取的信息
            public string LoadXML(string xmlPath)
            {
                StreamReader r = File.OpenText(xmlPath);
                string info = r.ReadToEnd();
                r.Close();
                return info;
            }
        }



        private void OptionWindowActivated(object sender, EventArgs e)
        {
            /*settings.isAutoStart = false;       //这两条是测试语句
            settings.isUsingServer = false;*/
            settings = new Settings();
            settings = new Settings(false, false, ("crtl", "E"), SchemeType.Caesar, SchemeType.Caesar, SchemeType.Caesar);
            
            

            /*TODO: 
             * 判断设置文件/CommonData/settings.xml是否存在
             * 不存在则创建，存在则以xml的反序列化方式读入生成Settings类
             */
            if (!File.Exists(@"./CommonData/settings.xml"))
            {
                XMLSaveAndRead xmlHandle = new XMLSaveAndRead();

                //存储信息
                List<Settings> info = new List<Settings>();
                Settings obj1 = new Settings(false, false, ("crtl", "E"), SchemeType.Caesar, SchemeType.Caesar, SchemeType.Caesar);
                info.Add(obj1);
                //将info的类型List<Test>和自身info传入
                string xmlInfo = xmlHandle.SerializeObject<List<Settings>>(info);
                xmlHandle.CreateXML("./CommonData/settings.xml", xmlInfo);

            }
            else
            {
                XMLSaveAndRead xmlHandle = new XMLSaveAndRead();
                string doc = xmlHandle.LoadXML("./CommonData/settings.xml");
                List<Settings> info1 = (List<Settings>)xmlHandle.DeserializeObject<List<Settings>>(doc);
                for (int i = 0; i < info1.Count; i++)
                {
                    settings.isAutoStart = info1[i].isAutoStart;
                    settings.isUsingServer = info1[i].isUsingServer;
                    settings.shortCutKey = info1[i].shortCutKey;
                    settings.encryptType = info1[i].encryptType;
                    settings.decryptType = info1[i].decryptType;
                    settings.breakType = info1[i].breakType;
                }
            }

            //更新slide的状态
            if (!this.settings.isAutoStart)
            {
                BitmapImage bi1 = new BitmapImage();
                bi1.BeginInit();
                bi1.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
                bi1.EndInit();
                this.isAutoStartSlide.Source = bi1;
            }

            if (!this.settings.isUsingServer)
            {
                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
                bi2.EndInit();
                this.isUsingServerSlide.Source = bi2;
            }

            defaultDict = new Dictionary<string, SchemeType>()
            {
                {"凯撒算法", SchemeType.Caesar},
                {"仿射算法", SchemeType.Affine},
                {"栅栏算法", SchemeType.RailFence},
                {"置换算法", SchemeType.Substitution}
            };

            EncryptComboBox.ItemsSource = defaultDict;
            EncryptComboBox.DisplayMemberPath = "Key";
            EncryptComboBox.SelectedValuePath = "Value";
            EncryptComboBox.SelectedIndex = settings.encryptTypeIndex;

            DecryptComboBox.ItemsSource = defaultDict;
            DecryptComboBox.DisplayMemberPath = "Key";
            DecryptComboBox.SelectedValuePath = "Value";
            DecryptComboBox.SelectedIndex = settings.decryptTypeIndex;

            BreakComboBox.ItemsSource = defaultDict;
            BreakComboBox.DisplayMemberPath = "Key";
            BreakComboBox.SelectedValuePath = "Value";
            BreakComboBox.SelectedIndex = settings.breakTypeIndex;


        }

        private void shortCutKeyTextBox_Change(object sender, DependencyPropertyChangedEventArgs e)
        {
            //暂时不需要写
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)       //绑定了3个ComboBox
        {
            var comboBox = sender as ComboBox;
            //TODO: 根据更换的选项更新settings

        }

        private void OptionWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: 这里为窗口正在关闭时的回调函数，在这里用xml序列化方式写入CommonData/setting.xml
            XMLSaveAndRead xmlHandle = new XMLSaveAndRead();

            //存储信息
            List<Settings> info = new List<Settings>();
            Settings obj1 = new Settings(this.settings.isAutoStart, this.settings.isUsingServer, this.settings.shortCutKey, this.settings.encryptType, this.settings.decryptType, this.settings.breakType);
            info.Add(obj1);
            //将info的类型List<Test>和自身info传入
            string xmlInfo = xmlHandle.SerializeObject<List<Settings>>(info);
            xmlHandle.CreateXML("./CommonData/settings.xml", xmlInfo);
        }

        private void isAutoStartMouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            if (this.settings.isAutoStart)
            {
                bi.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
            }
            else
            {
                bi.UriSource = new Uri(@"/assets/滑动开关-开启.png", UriKind.Relative);
            }
            bi.EndInit();
            image.Source = bi;
            this.settings.isAutoStart = !this.settings.isAutoStart;
        }

        private void isUsingServerMouseDown(object sender, MouseButtonEventArgs e)
        {
            //TODO: 是否使用服务器的slide被点击，翻转图片，修改settings
            var image = sender as Image;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();

            if (this.settings.isUsingServer)
            {
                bi.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
            }
            else
            {
                bi.UriSource = new Uri(@"/assets/滑动开关-开启.png", UriKind.Relative);
            }
            bi.EndInit();
            image.Source = bi;
            this.settings.isUsingServer = !this.settings.isUsingServer;
        }
    }

    public class Settings
    {
        public bool isAutoStart;                //是否自动开机
        public bool isUsingServer;              //是否使用服务器
        public (string, string) shortCutKey;    //采用A+B模式，如：Ctrl+E
        public SchemeType encryptType;          //默认加密算法
        public SchemeType decryptType;          //默认解密算法
        public SchemeType breakType;            //默认破解算法

        public Settings()
        {

        }


        public Settings(bool isAutoStart, bool isUsingServer, (string, string) shortCutKey, SchemeType encryptType, SchemeType decryptType, SchemeType breakType)
        {
            this.isAutoStart = isAutoStart;
            this.isUsingServer = isUsingServer;
            this.shortCutKey = shortCutKey;
            this.encryptType = encryptType;
            this.decryptType = decryptType;
            this.breakType = breakType;
        }

        private int SchemeTypeToIndex(SchemeType st)
        {
            switch (st)
            {
                case SchemeType.Caesar:
                    return 0;
                case SchemeType.Substitution:
                    return 1;
                case SchemeType.RailFence:
                    return 2;
                case SchemeType.Affine:
                    return 3;
                default:
                    return 0;
            }
        }

        public int encryptTypeIndex
        {
            get
            {
                return SchemeTypeToIndex(encryptType);
            }
        }

        public int decryptTypeIndex
        {
            get
            {
                return SchemeTypeToIndex(decryptType);
            }
        }

        public int breakTypeIndex
        {
            get
            {
                return SchemeTypeToIndex(breakType);
            }
        }
    }
}
