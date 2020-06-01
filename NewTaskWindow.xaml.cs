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
            OperateWindow operateWindow = new OperateWindow(task);
            task.Name = EncodeTitle.Text.ToString();
            task.OriginText = Plain.Text;
            task.ResultText = null;
            if (EncodeScheme.SelectedIndex == 0) { task.type = SchemeType.Caesar; }
            else if (EncodeScheme.SelectedIndex == 1) { task.type = SchemeType.Substitution; }
            else if (EncodeScheme.SelectedIndex == 2) { task.type = SchemeType.RailFence; }
            else if (EncodeScheme.SelectedIndex == 3) { task.type = SchemeType.Affine; }
            task.Date = DateTime.Now;
            task.Key = EncodeKey.Text;

            
            operateWindow.TaskTitle.Content = "任务名："+task.Name;
            operateWindow.OptType.Content = "操作：" + task.OptType;
            operateWindow.SchemeType.Content = "算法：" + task.type.ToString();
            operateWindow.Key.Content = "密钥：" + task.Key;
            operateWindow.Text.Text = "明文：" + task.OriginText;
            operateWindow.Date.Content = "创建时间：" + task.Date.ToString();
            operateWindow.Show();
            
            //CommonData.Tasks.Add(task);
        }

        private void DecodeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
