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

            this.settings = CommonData.settings;        //绑定全局settings
        }
        
        private void OptionWindowActivated(object sender, EventArgs e)
        {
            //更新slide的状态
            if (this.settings.isAutoStart)
            {
                BitmapImage bi1 = new BitmapImage();
                bi1.BeginInit();
                bi1.UriSource = new Uri(@"/assets/滑动开关-开启.png", UriKind.Relative);
                bi1.EndInit();
                this.isAutoStartSlide.Source = bi1;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri(@"/assets/开机-开启.png", UriKind.Relative);
                bi2.EndInit();
                this.autoStartImg.Source = bi2;
            }

            if (!this.settings.isUsingServer)
            {
                BitmapImage bi3 = new BitmapImage();
                bi3.BeginInit();
                bi3.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
                bi3.EndInit();
                this.isUsingServerSlide.Source = bi3;

                BitmapImage bi4 = new BitmapImage();
                bi4.BeginInit();
                bi4.UriSource = new Uri(@"/assets/服务器-关闭.png", UriKind.Relative);
                bi4.EndInit();
                this.usingServerImg.Source = bi4;
            }

            defaultDict = new Dictionary<string, SchemeType>()
            {
                {"凯撒算法", SchemeType.Caesar},
                {"置换算法", SchemeType.Substitution},
                {"栅栏算法", SchemeType.RailFence},
                {"仿射算法", SchemeType.Affine},
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

            //更新TextBox状态
            clipDefaultKeyTextBox.Text = settings.clipDefaultKey;
        }

        private void shortCutKeyTextBox_Change(object sender, DependencyPropertyChangedEventArgs e)
        {
            //暂时不需要写
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)       //绑定了3个ComboBox
        {
            var comboBox = sender as ComboBox;
            
            if (comboBox.Name == "EncryptComboBox")
            {
                settings.encryptType = (SchemeType)comboBox.SelectedValue;
            }
            else if (comboBox.Name == "DecryptComboBox")
            {
                settings.decryptType = (SchemeType)comboBox.SelectedValue;
            }
            else if (comboBox.Name == "BreakComboBox")
            {
                settings.breakType = (SchemeType)comboBox.SelectedValue;
            }
        }

        private void OptionWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //这里为窗口正在关闭时的回调函数，在这里用xml序列化方式写入setting.xml
            XMLSaveAndRead xmlHandle = new XMLSaveAndRead();

            //存储信息
            List<Settings> info = new List<Settings>();
            info.Add(this.settings);
            //将info的类型List<Test>和自身info传入
            string xmlInfo = xmlHandle.SerializeObject<List<Settings>>(info);
            string xmlPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            xmlPath += "settings.xml";
            xmlHandle.CreateXML(xmlPath, xmlInfo);
        }

        private void isAutoStartMouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            BitmapImage bi1 = new BitmapImage();
            BitmapImage bi2 = new BitmapImage();
            bi1.BeginInit();
            bi2.BeginInit();

            if (this.settings.isAutoStart)
            {
                bi1.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
                bi2.UriSource = new Uri(@"/assets/开机-关闭.png", UriKind.Relative);
            }
            else
            {
                bi1.UriSource = new Uri(@"/assets/滑动开关-开启.png", UriKind.Relative);
                bi2.UriSource = new Uri(@"/assets/开机-开启.png", UriKind.Relative);
            }
            bi1.EndInit();
            bi2.EndInit();
            image.Source = bi1;
            autoStartImg.Source = bi2;
            this.settings.isAutoStart = !this.settings.isAutoStart;
        }

        private void isUsingServerMouseDown(object sender, MouseButtonEventArgs e)
        {
            //TODO: 是否使用服务器的slide被点击，翻转图片，修改settings
            var image = sender as Image;
            BitmapImage bi1 = new BitmapImage();
            BitmapImage bi2 = new BitmapImage();
            bi1.BeginInit();
            bi2.BeginInit();

            if (this.settings.isUsingServer)
            {
                bi1.UriSource = new Uri(@"/assets/滑动开关-关闭.png", UriKind.Relative);
                bi2.UriSource = new Uri(@"/assets/服务器-关闭.png", UriKind.Relative);
            }
            else
            {
                bi1.UriSource = new Uri(@"/assets/滑动开关-开启.png", UriKind.Relative);
                bi2.UriSource = new Uri(@"/assets/服务器-开启.png", UriKind.Relative);
            }
            bi1.EndInit();
            bi2.EndInit();
            image.Source = bi1;
            usingServerImg.Source = bi2;
            this.settings.isUsingServer = !this.settings.isUsingServer;
        }

        private void clipDefaultKeyChanged(object sender, TextChangedEventArgs e)
        {
            var clipDeafultKeyTextBox = sender as TextBox;

            if (clipDeafultKeyTextBox.Text != null && this.settings != null && clipDeafultKeyTextBox.Text != "")
            {
                this.settings.clipDefaultKey = clipDeafultKeyTextBox.Text;
            }
        }
    }

    public class Settings
    {
        public bool isAutoStart;                //是否自动开机
        public bool isUsingServer;              //是否使用服务器
        public (string, string) shortCutKey;    //采用A+B模式，如：Ctrl+E
        public string clipDefaultKey;           //剪贴板加密的默认密钥
        public SchemeType encryptType;          //默认加密算法
        public SchemeType decryptType;          //默认解密算法
        public SchemeType breakType;            //默认破解算法

        public Settings() { }

        public Settings(bool isAutoStart, bool isUsingServer, (string, string) shortCutKey, string clipDefaultKey, SchemeType encryptType, SchemeType decryptType, SchemeType breakType)
        {
            this.isAutoStart = isAutoStart;
            this.isUsingServer = isUsingServer;
            this.shortCutKey = shortCutKey;
            this.clipDefaultKey = clipDefaultKey;
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
}
