using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Threading.Tasks;

namespace 独占文件
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindow_Closing;
        }
        private TaskFactory _taskFactory = new TaskFactory();
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (_filestream != null)
                    _filestream.Close();
            }
            catch
            {
            }
            finally { }
        }
        FileStream _filestream = null;
        /// <summary>
        /// Noshare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Task _task = _taskFactory.StartNew(() =>
            {
                try
                {
                    if (!File.Exists(FilePath.Text))
                        return;
                    string filepath = FilePath.Text;
                    _filestream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None);
                    byte[] imgData = new byte[(int)_filestream.Length];
                    _filestream.Read(imgData, 0, imgData.Length);
                }
                catch (Exception ex)
                {
                }
            });
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            Task _task = _taskFactory.StartNew(() =>
            {
                result_txt.Content = "删除中。。。";
                try
                {
                    if (!Directory.Exists(FilePath.Text) && !File.Exists(FilePath.Text))
                        return;
                    if (File.Exists(FilePath.Text))
                    {
                        var fileinfo = new FileInfo(FilePath.Text);
                        fileinfo.Delete();
                    }
                    if (Directory.Exists(FilePath.Text))
                    {
                        var _dirinfo = new DirectoryInfo(FilePath.Text);
                        _dirinfo.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                    result_txt.Content = "删除失败";
                }
                result_txt.Content = "删除成功";
            });
        }

        private void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            FilePath.Text = "";
        }

        private void Move_btn_Click(object sender, RoutedEventArgs e)
        {
            Task _task = _taskFactory.StartNew(() =>
            {

                try
                {

                    string sourcepath = FilePath.Text;
                    string destination = FilePath_dest.Text;
                    if (File.Exists(sourcepath))
                    {
                        FileInfo _fileinfo = new FileInfo(sourcepath);
                        _fileinfo.MoveTo(destination);
                    }
                    if (Directory.Exists(sourcepath))
                    {
                        DirectoryInfo _dirinfo = new DirectoryInfo(sourcepath);
                        _dirinfo.MoveTo(destination);
                    }
                    else
                    {
                        result_txt.Content = "File or Directory Not Exist";
                    }
                }
                catch (Exception ex)
                {
                    result_txt.Content = ex.Message;
                }
            });
        }

        private void copy_btn_Click(object sender, RoutedEventArgs e)
        {
            Task _task = _taskFactory.StartNew(() =>
            {

                try
                {
                    string filesource = FilePath.Text;
                    string filedest = FilePath_dest.Text;
                    File.Copy(filesource, filedest);
                }
                catch (Exception ex)
                {
                    result_txt.Content = ex.Message;
                }
            });
        }
    }
}
