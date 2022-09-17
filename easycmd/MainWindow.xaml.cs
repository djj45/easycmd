using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        ObservableCollection<string> files = new ObservableCollection<string>();
        ObservableCollection<string> cmdNames = new ObservableCollection<string>();
        ObservableCollection<string> cmdCmds = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            LoadCmd();

            FileListBox.ItemsSource = files;
            CmdListBox.ItemsSource = cmdNames;
        }

        private void FileListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePath = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string path in filePath)
                {
                    files.Add(path);
                }
                CmdListBox.Focus();
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
            OpenNewWindow();
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
            cmdNames.RemoveAt(CmdListBox.SelectedIndex);

            using (StreamWriter sw = new StreamWriter("1.txt"))
            {
                for (int i = 0; i < cmdNames.Count; i++)
                {
                    sw.WriteLine(cmdNames[i] + "<" + cmdCmds[i]);
                }
            }
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
            if (files.Count == 0)
            {
                e.CanExecute = false;
            }
            else
            {
                e.CanExecute = true;
            }
        }

        public void LoadCmd()
        {
            cmdNames.Clear();
            cmdCmds.Clear();

            using (StreamReader sr = new StreamReader("1.txt"))
            {
                string cmdLine;
                while ((cmdLine = sr.ReadLine()) != null)
                {
                    ListBoxCmd cmd = new ListBoxCmd(cmdLine.Split('<')[0], cmdLine.Split('<')[1]);
                    cmdNames.Add(cmd.Name);
                    cmdCmds.Add(cmd.Command);
                }
            }
        }

        private void EditCmdButton_Click(object sender, RoutedEventArgs e)
        {
            OpenNewWindow(cmdCmds, cmdNames, CmdListBox.SelectedIndex);
        }

        private void OpenNewWindow()
        {
            CmdWindow cmdWindow = new CmdWindow();
            cmdWindow.Top = Top + 20;
            cmdWindow.Left = Left + 20;
            cmdWindow.isSaveCmd += LoadCmd;
            cmdWindow.ShowDialog();
        }

        private void OpenNewWindow(ObservableCollection<string> cmdCmds, ObservableCollection<string> cmdNames, int index)
        {
            CmdWindow cmdWindow = new CmdWindow(cmdCmds, cmdNames, index);
            cmdWindow.Top = Top + 20;
            cmdWindow.Left = Left + 20;
            cmdWindow.isSaveCmd += LoadCmd;
            cmdWindow.ShowDialog();
        }
    }
}
