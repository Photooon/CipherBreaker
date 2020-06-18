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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CipherBreaker
{
    /// <summary>
    /// DecodePage.xaml 的交互逻辑
    /// </summary>
    public partial class DecodePage : Page
    {
        public DecodePage(Task task)
        {
            InitializeComponent();
            this.task = task;
            this.scheme = Scheme.NewScheme(task.type, task.ResultText, task.OriginText, task.Key);
            this.TaskTitle.Content = task.ToString();
            this.SchemeType.Content = task.type.ToString();
            this.Key.Content = task.Key;
            this.Text.Text = task.OriginText;
            this.Date.Text = task.Date.ToString();
            this.Result.Text = task.ResultText;
            if (task.ResultText != null)
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
        private bool flag = true;
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                StartButton.Content = "暂  停";
                (task.ResultText, _) = scheme.Decode(scheme.Cipher, task.Key);
                Result.Text = task.ResultText;
                StartButton.Content = "开  始";
            }
            else
            {
                StartButton.Content = "开  始";
                flag = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(Result.Text, true);
        }
    }
}
