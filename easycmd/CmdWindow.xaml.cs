using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace easycmd
{
    /// <summary>
    /// CmdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CmdWindow : Window
    {
        ObservableCollection<string> CmdNames { get; set; }
        ObservableCollection<string> CmdCmds { get; set; }
        ObservableCollection<string> CmdRunWindows { get; set; }
        ObservableCollection<string> CmdExits { get; set; }
        public delegate void IsSaveCmd(string path);
        public IsSaveCmd isSaveCmd;
        public int Index { get; set; }
        string CmdGroupPath { get; set; }
        List<string> comboBoxList1 = new List<string>() { "cmd", "powershell"};
        List<string> comboBoxList2 = new List<string>() { "不关闭", "关闭" };

        public CmdWindow(ObservableCollection<string> cmdCmds, ObservableCollection<string> cmdNames, ObservableCollection<string> cmdRunWindows, ObservableCollection<string> cmdExits, int index, string cmdGroupPath)
        {   //编辑
            InitializeComponent();
            CmdCmds = cmdCmds;
            CmdNames = cmdNames;
            CmdRunWindows = cmdRunWindows;
            CmdExits = cmdExits;
            Index = index;
            CmdGroupPath = cmdGroupPath;
            CmdTextBox.Text = cmdCmds[Index];
            NameTextBox.Text = cmdNames[Index];
            comboBox1.ItemsSource = comboBoxList1;
            comboBox2.ItemsSource = comboBoxList2;
            comboBox1.SelectedValue = cmdRunWindows[Index];
            comboBox2.SelectedValue = cmdExits[Index];
        }

        public CmdWindow(string cmdGroupPath)
        {   //新建
            InitializeComponent();
            Index = -1;
            CmdGroupPath = cmdGroupPath;
            comboBox1.ItemsSource = comboBoxList1;
            comboBox2.ItemsSource = comboBoxList2;
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CmdTextBox.Text) && !string.IsNullOrEmpty(NameTextBox.Text))
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string cmd = CmdTextBox.Text;
            string name = NameTextBox.Text;
            string runWindow = comboBox1.Text;
            string exit = comboBox2.Text;

            if (Index == -1)
            {
                using (StreamWriter sw = new StreamWriter(CmdGroupPath, true))
                {
                    sw.WriteLine(name + '|' + cmd + '|' + runWindow + '|' + exit);
                }
                SavedTextBlock.Visibility = Visibility.Visible;
                isSaveCmd(CmdGroupPath);
            }
            else
            {
                CmdNames.RemoveAt(Index);
                CmdCmds.RemoveAt(Index);
                CmdRunWindows.RemoveAt(Index);
                CmdExits.RemoveAt(Index);
                CmdNames.Insert(Index, name);
                CmdCmds.Insert(Index, cmd);
                CmdRunWindows.Insert(Index, runWindow);
                CmdExits.Insert(Index, exit);

                using (StreamWriter sw = new StreamWriter(CmdGroupPath))
                {
                    for (int i = 0; i < CmdNames.Count; i++)
                    {
                        sw.WriteLine(CmdNames[i] + "|" + CmdCmds[i] + "|" + CmdRunWindows[i] + "|" + CmdExits[i]);
                    }
                }
                SavedTextBlock.Visibility = Visibility.Visible;
                isSaveCmd(CmdGroupPath);
            }
        }
    }
}
