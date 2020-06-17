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

        private void OptionWindowActivated(object sender, EventArgs e)
        {
            settings = new Settings();
            settings.isAutoStart = false;       //这两条是测试语句
            settings.isUsingServer = false;
            /*TODO: 
             * 判断设置文件/CommonData/settings.xml是否存在
             * 不存在则创建，存在则以xml的反序列化方式读入生成Settings类
             */

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
