using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    /// BreakPage.xaml 的交互逻辑
    /// </summary>
    public partial class BreakPage : Page
    {
        public BreakPage(Task task)
        {
            InitializeComponent();
            this.task = task;
            this.scheme = Scheme.NewScheme(task.type, task.ResultText, task.OriginText, task.Key);
            this.TaskTitle.Content = task.ToString();
            this.SchemeType.Content = task.type.ToString();
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
        private bool isStarted = false;
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isStarted)
            {
                StartButton.Content = "暂  停";
            }
            else
            {
                StartButton.Content = "开  始";
                await scheme.BreakAsync();
                PrintCurrentResult();
                isStarted = true;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(Result.Text, true);
        }

        private async void PrintCurrentResult()
        {
            string log = "";
            while(!scheme.ProcessLog.IsEmpty)
            {
                scheme.ProcessLog.TryDequeue(out log);
                if (log == "")
                    break;
                Result.Text = log;
            }
        }
    }
}
