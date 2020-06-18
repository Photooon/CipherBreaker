using System;
using System.Collections.Generic;
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
    /// OperateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OperateWindow : Window
    {
        public OperateWindow(Task task)
        {
            this.task = task;

            InitializeComponent();
            this.scheme = Scheme.NewScheme(task.type, task.OriginText, task.ResultText, task.Key);
        }

        private Task task;
        private Scheme scheme;

        private void NewStartButton_Click(object sender, MouseButtonEventArgs e)
        {
            bool ok;
            if (task.OptType == OperationType.Encode)
            {
                (ResultText.Text, ok) = scheme.Encode(scheme.Plain, task.Key);
                ResultText.Text = "密文：" + ResultText.Text;
            }
            else if (task.OptType == OperationType.Decode)
            {
                (ResultText.Text, ok) = scheme.Decode(scheme.Plain, task.Key);
                ResultText.Text = "明文：" + ResultText.Text;
            }
            else if (task.OptType == OperationType.Break)
            {

            }
            NewStartButton.IsEnabled = false;
        }

        private void NewStartButton_Enter(object sender, MouseEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/assets/开始-悬停.png", UriKind.Relative);
            bi.EndInit();
            this.NewStartButton.Source = bi;
        }

        private void NewStartButton_Leave(object sender, MouseEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/assets/开始-默认.png", UriKind.Relative);
            bi.EndInit();
            this.NewStartButton.Source = bi;
        }

        private void DeleteButton_Enter(object sender, MouseEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/assets/垃圾桶-悬停.png", UriKind.Relative);
            bi.EndInit();
            this.DeleteButton.Source = bi;
        }

        private void DeleteButton_Leave(object sender, MouseEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/assets/垃圾桶-默认.png", UriKind.Relative);
            bi.EndInit();
            this.DeleteButton.Source = bi;
        }
    }
}
