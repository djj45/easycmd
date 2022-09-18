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
using System.Windows.Shapes;
using System.IO;

namespace easycmd
{
    /// <summary>
    /// cmdGroup.xaml 的交互逻辑
    /// </summary>
    public partial class CmdGroupWindow : Window
    {
        ObservableCollection<string> Group { get; set; }
        public delegate void IsSaveGroup();
        public IsSaveGroup isSaveGroup;
        public CmdGroupWindow(ObservableCollection<string> group)
        {
            InitializeComponent();
            Group = group;
        }

        private void SaveCmdGroup_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NameTextBox.Text) && !Group.Contains(NameTextBox.Text))
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
            using (StreamWriter sw = new StreamWriter(@"config\group\" + NameTextBox.Text + ".txt"))
            {

            }
            SavedTextBlock.Visibility = Visibility.Visible;
            isSaveGroup();
        }
    }
}
