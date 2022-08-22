using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace easycmd
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string s2 in s)
                {
                    FileListBox.Items.Add(s2);
                }
            }
        }

        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileListBox.Items.Remove(FileListBox.SelectedItem);
        }

        private void RemoveAllFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileListBox.Items.Clear();
        }

    }
}
