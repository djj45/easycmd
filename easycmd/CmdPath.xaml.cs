using System.IO;
using System.Windows;
using System.Windows.Input;

namespace easycmd
{
    /// <summary>
    /// CmdPath.xaml 的交互逻辑
    /// </summary>
    public partial class CmdPath : Window
    {
        string OutputPath { get; set; }
        public delegate void IsSavePath();
        public IsSavePath isSavePath;
        public CmdPath(string path)
        {
            InitializeComponent();
            PathTextBox.Text = path;
        }

        private void SaveCmdGroup_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PathTextBox.Text))
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
            OutputPath = PathTextBox.Text;
            using (StreamWriter sw = new StreamWriter(@"config\path\path.txt"))
            {
                sw.WriteLine(OutputPath);
            }
            SavedTextBlock.Visibility = Visibility.Visible;
            isSavePath();
        }
    }
}
