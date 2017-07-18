using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for CreateDialog.xaml
    /// </summary>
    public partial class CreateDialog : Window
    {
        public CreateDialog()
        {
            InitializeComponent();
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = MainWindow.getSelectedItem();
            string newPath;

            if (selectedItem == null)
                return;

            if (createFile.IsChecked == true) {
                if (textAnswer == null || !Regex.IsMatch(textAnswer.Text, @"[a-zA-Z0-9_~-]{1,8}\.(txt|php|html)"))
                {
                    MessageBoxResult result = MessageBox.Show("Please, enter correct file name.", "Error", MessageBoxButton.OK);
                }
                else {
                    newPath = System.IO.Path.Combine(selectedItem.Tag.ToString(), textAnswer.Text);

                        if (!File.Exists(newPath))
                        {
                            using (File.Create(newPath)) ;
                            var newTreeItem = new TreeViewItem();
                            FileInfo fi = new FileInfo(newPath);
                            if (readOnly.IsChecked == true)
                            {
                                fi.Attributes |= FileAttributes.ReadOnly;
                            }
                            if (archive.IsChecked == false)
                            {
                                fi.Attributes &= ~FileAttributes.Archive;
                            }
                            if (hidden.IsChecked == true)
                            {
                                fi.Attributes |= FileAttributes.Hidden;
                            }
                            if (system.IsChecked == true)
                            {
                                fi.Attributes |= FileAttributes.System;
                            }
                            newTreeItem.Tag = fi;
                            newTreeItem.Header = new FileInfo(fi.Name);
                            selectedItem.Items.Add(newTreeItem);
                            this.Close();
                    }
                   else
                    { 
                        MessageBoxResult result = MessageBox.Show("File already exists.", "Error", MessageBoxButton.OK);
                    }
                }
            }
            else if (createDirectory.IsChecked == true) {
                {
                    if (textAnswer == null || !Regex.IsMatch(textAnswer.Text, @"[a-zA-Z0-9_~-]{1,50}"))
                    {
                        MessageBoxResult result = MessageBox.Show("Please, enter correct directory name.", "Error", MessageBoxButton.OK);
                    }
                    else {
                        newPath = System.IO.Path.Combine(selectedItem.Tag.ToString(), textAnswer.Text);

                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                            var newTreeItem = new TreeViewItem();
                            DirectoryInfo di = new DirectoryInfo(newPath);
                            if (readOnly.IsChecked == true)
                            {
                                di.Attributes |= FileAttributes.ReadOnly;
                            }
                            if (archive.IsChecked == true)
                            {
                                di.Attributes |= FileAttributes.Archive;
                            }
                            if (hidden.IsChecked == true)
                            {
                                di.Attributes |= FileAttributes.Hidden;
                            }
                            if (system.IsChecked == true)
                            {
                                di.Attributes |= FileAttributes.System;
                            }
                            newTreeItem.Tag = di;
                            newTreeItem.Header = new FileInfo(di.Name);
                            selectedItem.Items.Add(newTreeItem);
                            this.Close();
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show("Directory already exists.", "Error", MessageBoxButton.OK);
                        }
                    }
                }
            }
        }
    }
}
