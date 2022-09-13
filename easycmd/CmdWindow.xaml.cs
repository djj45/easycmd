using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Shapes;

namespace easycmd
{
    /// <summary>
    /// CmdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CmdWindow : Window
    {
        ObservableCollection<string> CmdNames { get; set; }
        ObservableCollection<string> CmdCmds { get; set; }
        public delegate void IsSaveCmd();
        public IsSaveCmd isSaveCmd;
        public int Index { get; set; }

        public CmdWindow(ObservableCollection<string> cmdCmds, ObservableCollection<string> cmdNames, int index)
        {
            InitializeComponent();
            CmdCmds = cmdCmds;
            CmdNames = cmdNames;
            Index = index;
            CmdTextBox.Text = cmdCmds[Index];
            NameTextBox.Text = cmdNames[Index];
        }

        public CmdWindow()
        {
            InitializeComponent();
            Index = -1;
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

            if (Index == -1)
            {
                using (StreamWriter sw = new StreamWriter("1.txt", true))
                {
                    sw.WriteLine(name + '<' + cmd);
                    MessageBox.Show("保存成功");
                }
                isSaveCmd();
            }
            else
            {
                CmdNames.RemoveAt(Index);
                CmdCmds.RemoveAt(Index);
                CmdNames.Insert(Index, name);
                CmdCmds.Insert(Index, cmd);

                using (StreamWriter sw = new StreamWriter("1.txt"))
                {
                    for (int i = 0; i < CmdNames.Count; i++)
                    {
                        sw.WriteLine(CmdNames[i] + "<" + CmdCmds[i]);
                    }
                }
                MessageBox.Show("保存成功");
                isSaveCmd();
            }
        }
    }
}
