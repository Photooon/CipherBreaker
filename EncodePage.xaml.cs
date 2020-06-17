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
            this.task = task;
            InitializeComponent();
            this.scheme = Scheme.NewScheme(task.type, task.OriginText, task.ResultText, task.Key);
        }

        private Task task;
        private Scheme scheme;
        private bool flag = true;
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if(flag)
            {
                StartButton.Content = "暂  停";
                (Result.Text, _) = scheme.Encode(scheme.Plain, task.Key);
                this.ProgressBar.Value = ProgressBar.Maximum;
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
