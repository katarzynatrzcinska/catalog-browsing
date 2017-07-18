using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        private static TreeViewItem selectedItemForCreation;
        private static string selectedPath;
        public MainWindow() { }

        void open_click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            selectedPath = dialog.SelectedPath;
            LoadDirectory(treeView, dialog.SelectedPath);
        }

        void exit_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadDirectory(System.Windows.Controls.TreeView treeView, string selectedPath)
        {
            treeView.Items.Clear();
            treeView.Items.Add(addDirectoryItem(new DirectoryInfo(selectedPath)));
        }

        private TreeViewItem addDirectoryItem(DirectoryInfo directoryInfo)
        {
            var result = new TreeViewItem();
            result.Header = directoryInfo.Name;
            result.Tag = directoryInfo.FullName;

            foreach (var directory in directoryInfo.GetDirectories())
            {
                result.Items.Add(addDirectoryItem(directory));
            }

            foreach (var file in directoryInfo.GetFiles())
            {
                result.Items.Add(addFileItem(file));
            }

            return result;
        }

        private TreeViewItem addFileItem(FileInfo fileInfo)
        {
            var result = new TreeViewItem();
            result.Header = fileInfo.Name;
            result.Tag = fileInfo.FullName;

            return result;
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = e.OriginalSource as DependencyObject;
            TreeViewItem item = GetDependencyObjectFromVisualTree(obj, typeof(TreeViewItem)) as TreeViewItem;
            if (item != null)
            {
                FileAttributes attr = File.GetAttributes(item.Tag.ToString());
                
                System.Windows.Controls.MenuItem menuItem1 = new System.Windows.Controls.MenuItem();
                menuItem1.Header = "Open";
                menuItem1.Click += new RoutedEventHandler(openFromContextMenu_click);

                System.Windows.Controls.MenuItem menuItem2 = new System.Windows.Controls.MenuItem();
                menuItem2.Header = "Delete";
                menuItem2.Click += new RoutedEventHandler(deleteFromContextMenu_click);

                System.Windows.Controls.MenuItem menuItem3 = new System.Windows.Controls.MenuItem();
                menuItem3.Header = "Create";
                menuItem3.Click += new RoutedEventHandler(createFromContextMenu_click);

                System.Windows.Controls.MenuItem menuItem4 = new System.Windows.Controls.MenuItem();
                menuItem4.Header = "Open localisation";
                menuItem4.Click += new RoutedEventHandler(openLocalisationFromContextMenu_click);

                System.Windows.Controls.MenuItem menuItem5 = new System.Windows.Controls.MenuItem();
                menuItem5.Header = "Count files";
                menuItem5.Click += new RoutedEventHandler(countFilesFromContextMenu_click);

                System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu() { };

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    menu.Items.Add(menuItem3);
                    menu.Items.Add(menuItem2);
                    menu.Items.Add(menuItem5);
                }
                else
                {
                    menu.Items.Add(menuItem1);
                    menu.Items.Add(menuItem2);
                    menu.Items.Add(menuItem4);
                }

                item.ContextMenu = menu;
            }
        }

        private static DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            var parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }

        void openLocalisationFromContextMenu_click(object sender, System.EventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)treeView.SelectedItem;

            if (selectedItem == null)
                return;

            System.Windows.Controls.TreeViewItem selectedItemParent = (System.Windows.Controls.TreeViewItem)selectedItem.Parent;
            Process.Start(selectedItemParent.Tag.ToString());
        }

        void countFilesFromContextMenu_click(object sender, System.EventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)treeView.SelectedItem;

            if (selectedItem == null)
                return;

            string path = selectedItem.Tag.ToString();
            DirectoryInfo di = new DirectoryInfo(path);

            long folderSize = 0;
            foreach (string file in Directory.GetFiles(path))
            {
                FileInfo finfo = new FileInfo(file);
                folderSize ++;
            }

            foreach (string dir in Directory.GetDirectories(path))
            {
                DirectoryInfo dinfo = new DirectoryInfo(dir);
                folderSize++;
            }

            System.Windows.Forms.MessageBox.Show("Number of files and directories: " + folderSize, "Results", MessageBoxButtons.OK);
        }

        void deleteFromContextMenu_click(object sender, System.EventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)treeView.SelectedItem;

            if (selectedItem == null)
                return;

            File.SetAttributes(selectedItem.Tag.ToString(), FileAttributes.Normal);

            if (File.GetAttributes(selectedItem.Tag.ToString()) == FileAttributes.Directory)
            {
                DirectoryInfo dr = new DirectoryInfo(selectedItem.Tag.ToString());
                ClearReadOnly(dr);
                dr.Delete(true);
            }
            else
            {
                FileInfo fi = new FileInfo(selectedItem.Tag.ToString());
                fi.Delete();
            }

            if (selectedItem.Tag.ToString() != selectedPath)
            {
                System.Windows.Controls.TreeViewItem selectedItemParent = (System.Windows.Controls.TreeViewItem)selectedItem.Parent;
                selectedItemParent.Items.Remove(selectedItem);
            }
            else
            {
                selectedItem.Items.Remove(selectedItem);
                selectedItem.Items.Clear();
            }

        }

        private void ClearReadOnly(DirectoryInfo parentDirectory)
        {
            if (parentDirectory != null)
            {
                parentDirectory.Attributes = FileAttributes.Normal;
                foreach (FileInfo fi in parentDirectory.GetFiles())
                {
                    fi.Attributes = FileAttributes.Normal;
                }
                foreach (DirectoryInfo di in parentDirectory.GetDirectories())
                {
                    ClearReadOnly(di);
                }
            }
        }

        void createFromContextMenu_click(object sender, RoutedEventArgs e)
        {
            selectedItemForCreation = (TreeViewItem)treeView.SelectedItem;

            if (selectedItemForCreation == null)
                return;

            CreateDialog cd = new CreateDialog();
            cd.Show();
        }

        void openFromContextMenu_click(object sender, System.EventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)treeView.SelectedItem;

            if (selectedItem == null)
                return;

            try
            {
                textBox.Clear();
                using (StreamReader sr = new StreamReader(selectedItem.Tag.ToString()))
                {
                    textBox.Text += sr.ReadToEnd();
                }
            }
            catch (IOException ex) { }
           
        }

        public static String getDosAttributes(string path)
        {
            FileAttributes attribs = File.GetAttributes(path);
            return ((attribs & FileAttributes.ReadOnly) > 0 ? "r" : "-") +
                ((attribs & FileAttributes.Archive) > 0 ? "a" : "-") +
                ((attribs & FileAttributes.Hidden) > 0 ? "h" : "-") +
                ((attribs & FileAttributes.System) > 0 ? "s" : "-");
        }

        public static TreeViewItem getSelectedItem()
        {
            return selectedItemForCreation;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = (TreeViewItem)treeView.SelectedItem;
            textBlock.Text = getDosAttributes(selectedItem.Tag.ToString());
        }
    }
    
}
