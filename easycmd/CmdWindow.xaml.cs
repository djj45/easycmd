using System;
using System.Collections.Generic;
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
        List<string> cmdNames = new List<string>();
        List<string> cmdCommands = new List<string>();
        public delegate void IsSaveCmd();
        public IsSaveCmd isSaveCmd;

        public CmdWindow()
        {
            InitializeComponent();
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
            LoadCmd();
            string command = CmdTextBox.Text;
            string name = NameTextBox.Text;
            bool canSave = true;

            foreach (string s in cmdCommands)
            {
                if (s == command)
                {
                    MessageBox.Show("命令重复");
                    canSave = false;
                }
            }
            foreach (string s in cmdNames)
            {
                if (s == name)
                {
                    MessageBox.Show("名称重复");
                    canSave = false;
                }
            }

            if (canSave)
            {
                using (StreamWriter sw = new StreamWriter("1.txt", true))
                {
                    sw.WriteLine(name + '<' + command);
                    MessageBox.Show("保存成功");
                }
                isSaveCmd();
            }
        }

        private void LoadCmd()
        {
            using (StreamReader sr = new StreamReader("1.txt"))
            {
                string cmdLine;
                while ((cmdLine = sr.ReadLine()) != null)
                {
                    Cmd cmd = new Cmd(cmdLine.Split('<')[0], cmdLine.Split('<')[1]);
                    cmdNames.Add(cmd.Name);
                    cmdCommands.Add(cmd.Command);
                }
            }
        }
    }
}
