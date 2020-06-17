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
            this.scheme = Scheme.NewScheme(task.type, task.OriginText, task.ResultText, task.Key);
        }

        private Task task;
        private Scheme scheme;
        private bool isStarted = true;
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (isStarted)
            {
                StartButton.Content = "暂  停";
                StartButton.Content = "开  始";
            }
            else
            {
                StartButton.Content = "开  始";
                Scheme.AsyncBreak caller = new Scheme.AsyncBreak(scheme.Break);
                caller.BeginInvoke(scheme.Plain, null, null);
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
