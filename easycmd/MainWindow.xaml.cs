using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        ObservableCollection<string> cmdRunWindows = new ObservableCollection<string>();
        ObservableCollection<string> cmdExits = new ObservableCollection<string>();
        ObservableCollection<string> cmdGroup = new ObservableCollection<string>();
        int cmdListBoxSeletedIndex = 0;
        string CmdGroupPath { get; set; }
        string OutputPath { get; set; }
        string RealCmd { get; set; }
        string ExitSetting { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CheckConfig();
            LoadGroup();
            LoadPath();

            FileListBox.ItemsSource = files;
            CmdListBox.ItemsSource = cmdNames;
            GroupComboBox.ItemsSource = cmdGroup;
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
                CreateCmd(GroupComboBox.SelectedValue.ToString());//@@if null?
            }
        }

        private void DeleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            files.RemoveAt(FileListBox.SelectedIndex);
            CreateCmd(GroupComboBox.SelectedValue.ToString());
        }

        private void ClearFileButton_Click(object sender, RoutedEventArgs e)
        {
            files.Clear();
            CreateCmd(GroupComboBox.SelectedValue.ToString());
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
            int index = CmdListBox.SelectedIndex;
            cmdNames.RemoveAt(index);
            cmdCmds.RemoveAt(index);
            cmdRunWindows.RemoveAt(index);
            cmdExits.RemoveAt(index);

            using (StreamWriter sw = new StreamWriter(CmdGroupPath))
            {
                for (int i = 0; i < cmdNames.Count; i++)
                {
                    sw.WriteLine(cmdNames[i] + "|" + cmdCmds[i] + "|" + cmdRunWindows[i] + "|" + cmdExits[i]);
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

        public void LoadCmd(string path)
        {
            cmdNames.Clear();
            cmdCmds.Clear();
            cmdRunWindows.Clear();
            cmdExits.Clear();

            using (StreamReader sr = new StreamReader(path))
            {
                string cmdLine;
                while ((cmdLine = sr.ReadLine()) != null)
                {
                    ListBoxCmd cmd = new ListBoxCmd(cmdLine.Split('|')[0], cmdLine.Split('|')[1], cmdLine.Split('|')[2], cmdLine.Split('|')[3]);
                    cmdNames.Add(cmd.Name);
                    cmdCmds.Add(cmd.Command);
                    cmdRunWindows.Add(cmd.RunWindow);
                    cmdExits.Add(cmd.Exit);
                }
            }

            CmdListBox.SelectedIndex = cmdListBoxSeletedIndex;
        }

        private void EditCmdButton_Click(object sender, RoutedEventArgs e)
        {
            cmdListBoxSeletedIndex = CmdListBox.SelectedIndex;
            OpenEditWindow();
        }

        private void OpenNewWindow()
        {
            CmdWindow cmdWindow = new CmdWindow(CmdGroupPath);
            cmdWindow.Top = Top + 20;
            cmdWindow.Left = Left + 20;
            cmdWindow.isSaveCmd += LoadCmd;
            cmdWindow.ShowDialog();
        }

        private void OpenEditWindow()
        {
            CmdWindow cmdWindow = new CmdWindow(cmdCmds, cmdNames, cmdRunWindows, cmdExits, CmdListBox.SelectedIndex, CmdGroupPath);
            cmdWindow.Top = Top + 20;
            cmdWindow.Left = Left + 20;
            cmdWindow.isSaveCmd += LoadCmd;
            cmdWindow.ShowDialog();
        }

        private void LoadGroup()
        {
            cmdGroup.Clear();
            string[] files = Directory.GetFiles(@"config\group");
            foreach (var file in files)
            {
                cmdGroup.Add(Path.GetFileName(file).Split('.')[0]);
            }

            GroupComboBox.SelectedValue = "常用";
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupComboBox.SelectedValue != null)
            {
                CmdGroupPath = @"config\group\" + GroupComboBox.SelectedValue.ToString() + ".txt";
                LoadCmd(CmdGroupPath);
                CmdListBox.SelectedIndex = 0;
                CreateCmd(GroupComboBox.SelectedValue.ToString());
            }
        }

        private void NewGroupButton_Click(object sender, RoutedEventArgs e)
        {
            CmdGroupWindow cmdGroupWindow = new CmdGroupWindow(cmdGroup);
            cmdGroupWindow.Top = Top + 20;
            cmdGroupWindow.Left = Left + 20;
            cmdGroupWindow.isSaveGroup += LoadGroup;
            cmdGroupWindow.ShowDialog();
        }

        private void SetPathButton_Click(object sender, RoutedEventArgs e)
        {
            CmdPath cmdPath = new CmdPath(OutputPath);
            cmdPath.Top = Top + 20;
            cmdPath.Left = Left + 20;
            cmdPath.isSavePath += LoadPath;
            cmdPath.ShowDialog();
        }

        private void LoadPath()
        {
            using (StreamReader sr = new StreamReader(@"config\path\path.txt"))
            {
                OutputPath = sr.ReadLine();
            }
        }

        private void CheckConfig()
        {
            if (!(Directory.Exists(Directory.GetCurrentDirectory() + @"\config\group")))
            {
                Directory.CreateDirectory(@"config\group");
                using (File.Create(@"config\group\常用.txt")) { }
                using (File.Create(@"config\group\批量.txt")) { }

            }
            if (!(Directory.Exists(Directory.GetCurrentDirectory() + @"\config\path")))
            {
                Directory.CreateDirectory(@"config\path");
                using (StreamWriter sw = new StreamWriter(@"config\path\path.txt"))
                {
                    sw.WriteLine(@"output\");
                }
            }
            if (!(Directory.Exists(Directory.GetCurrentDirectory() + @"\output")))
            {
                Directory.CreateDirectory("output");
            }
        }

        private void CreateCmd(string group)
        {
            if (cmdCmds.Count != 0 && CmdListBox.SelectedIndex != -1)
            {
                if (group == "批量")
                {
                    RealCmd = BulkCmd.Get(cmdCmds[CmdListBox.SelectedIndex], files);
                }
                else
                {
                    RealCmd = GetCmd.Get(cmdCmds[CmdListBox.SelectedIndex], files);
                }
                ExitSetting = ExitCmd.Get(cmdRunWindows[CmdListBox.SelectedIndex], cmdExits[CmdListBox.SelectedIndex]);
                CmdTextBox.Text = RealCmd;
            }
            else
            {
                CmdTextBox.Text = "";
            }
        }

        private void CmdListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreateCmd(GroupComboBox.SelectedValue.ToString());
        }
    }
}
