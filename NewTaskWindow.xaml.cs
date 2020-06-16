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
    /// NewTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        public NewTaskWindow()
        {
            InitializeComponent();
        }

        private void EncodeButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task();
            
            task.Name = EncodeTitle.Text;
            task.OriginText = Plain.Text;
            task.ResultText = null;
            task.OptType = OperationType.Encode;
            if (EncodeScheme.SelectedIndex == 0) { task.type = SchemeType.Caesar; }
            else if (EncodeScheme.SelectedIndex == 1) { task.type = SchemeType.Substitution; }
            else if (EncodeScheme.SelectedIndex == 2) { task.type = SchemeType.RailFence; }
            else if (EncodeScheme.SelectedIndex == 3) { task.type = SchemeType.Affine; }
            task.Date = DateTime.Now;
            task.Key = EncodeKey.Text;

            EncodePage encodePage = new EncodePage(task);
            encodePage.TaskTitle.Content += task.ToString();
            encodePage.SchemeType.Content += task.type.ToString();
            encodePage.Key.Content += task.Key;
            encodePage.Text.Text += "\n" + task.OriginText;
            encodePage.Date.Text += "\n" + task.Date.ToString();
            //MainWindow.ContentControl.Content = new Frame() { Content = encodePage };//在主窗体中创建新页
            CommonData.Tasks.Add(task);


            //OperateWindow operateWindow = new OperateWindow(task);

            //operateWindow.TaskTitle.Content = "任务名："+task.Name;
            //operateWindow.OptType.Content = "操作：" + task.OptType;
            //operateWindow.SchemeType.Content = "算法：" + task.type.ToString();
            //operateWindow.Key.Content = "密钥：" + task.Key;
            //operateWindow.Text.Text = "明文：" + task.OriginText;
            //operateWindow.Date.Content = "创建时间：" + task.Date.ToString();
            //operateWindow.Show();

            //CommonData.Tasks.Add(task);
        }

        private void DecodeButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task();

            task.Name = DecodeTitle.Text;
            task.ResultText = Cipher.Text;
            task.OriginText = null;
            task.OptType = OperationType.Decode;
            if (DecodeScheme.SelectedIndex == 0) { task.type=SchemeType.Caesar;}
            else if (DecodeScheme.SelectedIndex == 1) { task.type = SchemeType.Substitution; }
            else if (DecodeScheme.SelectedIndex == 2) { task.type = SchemeType.RailFence; }
            else if (DecodeScheme.SelectedIndex == 3) { task.type = SchemeType.Affine; }
            task.Date = DateTime.Now;
            task.Key = DecodeKey.Text;
            OperateWindow operateWindow = new OperateWindow(task);

            operateWindow.TaskTitle.Content = "任务名：" + task.Name;
            operateWindow.OptType.Content = "操作：" + task.OptType;
            operateWindow.SchemeType.Content = "算法：" + task.type.ToString();
            operateWindow.Key.Content = "密钥：" + task.Key;
            operateWindow.Text.Text = "密文：" + task.OriginText;
            operateWindow.Date.Content = "创建时间：" + task.Date.ToString();
            operateWindow.Show();
        }

        private void BreakButton_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task();

            task.Name = BreakTitle.Text;
            task.OriginText = BreakPlain.Text;
            task.ResultText = null;
            task.OptType = OperationType.Break;
            task.Date = DateTime.Now;
            OperateWindow operateWindow = new OperateWindow(task);

            operateWindow.TaskTitle.Content = "任务名：" + task.Name;
            operateWindow.OptType.Content = "操作：" + task.OptType;
            //operateWindow.SchemeType.Content = "算法：" + task.type.ToString();
            //operateWindow.Key.Content = "密钥：" + task.Key;
            //operateWindow.Text.Text = "密文：" + task.OriginText;
            operateWindow.SchemeType.IsEnabled = false;
            operateWindow.Key.IsEnabled = false;
            operateWindow.Text.IsEnabled = false;
            operateWindow.Date.Content = "创建时间：" + task.Date.ToString();
            operateWindow.Show();
        }
    }
}
