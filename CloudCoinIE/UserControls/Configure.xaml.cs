using Founders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace CloudCoinIE.UserControls
{
    /// <summary>
    /// Interaction logic for Configure.xaml
    /// </summary>
    public partial class Configure : UserControl
    {
        public Configure()
        {
            InitializeComponent();
            lblDirectory.Text = Properties.Settings.Default.WorkSpace;
        }

        private void cmdDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string sMessageBoxText = "Do you want to Change CloudCoin Folder?";
                    string sCaption = "My Test Application";

                    MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
                    MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

                    MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            /* ... */
                            lblDirectory.Text = dialog.SelectedPath;
                            Properties.Settings.Default.WorkSpace = dialog.SelectedPath+ Path.DirectorySeparatorChar;
                            Properties.Settings.Default.Save();
                            FileUtils fileUtils = FileUtils.GetInstance(Properties.Settings.Default.WorkSpace);
                            fileUtils.CreateDirectoryStructure();
                            string[] fileNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                            foreach (String fileName in fileNames)
                            {
                                if (fileName.Contains("jpeg"))
                                {
                                    try
                                    {
                                        string outputpath = Properties.Settings.Default.WorkSpace + "Templates" + Path.DirectorySeparatorChar + fileName.Substring(22);
                                        using (FileStream fileStream = File.Create(outputpath))
                                        {
                                            Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName).CopyTo(fileStream);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                            break;

                        case MessageBoxResult.No:
                            /* ... */
                            break;

                        case MessageBoxResult.Cancel:
                            /* ... */
                            break;
                    }
                }
            }
        }

        private void cmdBackup_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    copyFolders(dialog.SelectedPath);
                }
            }
        }

        private void copyFolders(string destination)
        {
            string SourcePath = Properties.Settings.Default.WorkSpace + Path.DirectorySeparatorChar + "Bank";
            string destinationPath = destination + Path.DirectorySeparatorChar + "Bank";
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, destinationPath));

            Directory.CreateDirectory(destinationPath);

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, destinationPath), true);


            SourcePath = Properties.Settings.Default.WorkSpace + Path.DirectorySeparatorChar + "Fracked";
            destinationPath = destination + Path.DirectorySeparatorChar + "Fracked";
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, destinationPath));

            Directory.CreateDirectory(destinationPath);

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, destinationPath), true);

            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            notifier.ShowInformation("CloudCoins Backup Completed.");


        }
        private void cmdRestore_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdShowFolders_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(MainWindow.rootFolder);
        }
    }
}
