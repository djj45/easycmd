using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<FileName> files = new ObservableCollection<FileName>();
        
        public MainWindow()
        {
            InitializeComponent();
            
            FileListBox.ItemsSource = files;
            FileListBox.DisplayMemberPath = "Name";
        }

        private void FileListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string path in filePath)
                {
                    FileName file = new FileName(path);
                    files.Add(file);
                }
            }
        }

        private void DeleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            files.RemoveAt(FileListBox.SelectedIndex);
        }

        private void ClearFileButton_Click(object sender, RoutedEventArgs e)
        {
            files.Clear();
        }

        private void NewCmdButton_Click(object sender, RoutedEventArgs e)
        {
            CmdWindow cmdWindow = new CmdWindow();
            cmdWindow.Top = Top + 20;
            cmdWindow.Left = Left + 20;
            cmdWindow.ShowDialog();
        }

        private void DeleteFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (FileListBox.SelectedItem != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void DeleteCmdButton_Click(object sender, RoutedEventArgs e)
        {
            CmdListBox.Items.Remove(CmdListBox.SelectedItem);
        }

        private void DeleteCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (CmdListBox.SelectedItem != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void ClearFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //bug:当文件列表只有一个文件时，清空按钮不可用
            if (files.Count == 0)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }
    }
}
