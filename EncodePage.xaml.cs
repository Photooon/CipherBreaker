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

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            bool ok;
            (Result.Text, ok) = scheme.Encode(scheme.Plain, task.Key);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
