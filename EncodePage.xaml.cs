using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CipherBreaker
{
    /// <summary>
    /// EncodePage.xaml 的交互逻辑
    /// </summary>
    public partial class EncodePage : Page
    {
        public EncodePage(Task task)
        {
            InitializeComponent();
            this.task = task;
            this.scheme = Scheme.NewScheme(task.type, task.OriginText, task.ResultText, task.Key);
            this.TaskTitle.Content = task.ToString();
            this.SchemeType.Content = Scheme.GetChineseSchemeTypeName(task.type);
            this.Key.Text = task.Key;
            this.Text.Text = task.OriginText;
            this.Date.Text = task.Date.ToString();
            this.Result.Text = task.ResultText;
            if (task.ResultText != null && task.ResultText.Length > 0)
            {
                this.Result.Text = task.ResultText;
                ProgressBar.Value = ProgressBar.Maximum;
            }
            else
            {
                this.Result.Text = "";
                ProgressBar.Value = 0;
            }
        }

        private Task task;
        private Scheme scheme;
        private bool isStarted = false;

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(Result.Text, true);
        }

        private void StartButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (isStarted)
            {
                // 暂停
                //StartButton.Content = "开  始";
            }
            else
            {
                isStarted = true;
                //StartButton.Content = "暂  停";

                (task.ResultText, _) = scheme.Encode(scheme.Plain, task.Key);
                this.ProgressBar.Value = ProgressBar.Maximum;
                Result.Text = task.ResultText;

                isStarted = false;
                //StartButton.Content = "开  始";
            }
        }

        private void StartButton_Enter(object sender, MouseEventArgs e)
        {
            if (!isStarted)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"/assets/开始-悬停.png", UriKind.Relative);
                bi.EndInit();
                this.StartButton.Source = bi;
            }
        }

        private void StartButton_Leave(object sender, MouseEventArgs e)
        {
            if (!isStarted)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"/assets/开始-默认.png", UriKind.Relative);
                bi.EndInit();
                this.StartButton.Source = bi;
            }
        }

        private void StartButton_Up(object sender, MouseButtonEventArgs e)
        {
            /*BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"/assets/开始-默认.png", UriKind.Relative);
            bi.EndInit();
            this.StartButton.Source = bi;*/
        }

        private void DeleteButton_Click(object sender, MouseButtonEventArgs e)
        {
            
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
