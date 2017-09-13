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
        FileUtils fileUtils;

        public void export(string backupDir)
        {
            fileUtils = MainWindow.fileUtils;


            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(fileUtils.BankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);

            //updateLog("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = bankTotals[1] + frackedTotals[1] + partialTotals[1];
            int exp_5 = bankTotals[2] + frackedTotals[2] + partialTotals[2];
            int exp_25 = bankTotals[3] + frackedTotals[3] + partialTotals[3];
            int exp_100 = bankTotals[4] + frackedTotals[4] + partialTotals[4];
            int exp_250 = bankTotals[5] + frackedTotals[5] + partialTotals[5];
            //Warn if too many coins

            if (exp_1 + exp_5 + exp_25 + exp_100 + exp_250 == 0)
            {
                Console.WriteLine("Can not export 0 coins");
                return;
            }

            //updateLog(Convert.ToString(bankTotals[1] + frackedTotals[1] + bankTotals[2] + frackedTotals[2] + bankTotals[3] + frackedTotals[3] + bankTotals[4] + frackedTotals[4] + bankTotals[5] + frackedTotals[5] + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]));

            if (((bankTotals[1] + frackedTotals[1]) + (bankTotals[2] + frackedTotals[2]) + (bankTotals[3] + frackedTotals[3]) + (bankTotals[4] + frackedTotals[4]) + (bankTotals[5] + frackedTotals[5]) + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]) > 1000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                Console.Out.WriteLine("Do not export stack files with more than 1000 notes. .");
                //updateLog("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                //updateLog("Do not export stack files with more than 1000 notes. .");

                Console.ForegroundColor = ConsoleColor.White;
            }//end if they have more than 1000 coins

            Console.Out.WriteLine("  Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?");           
            Exporter exporter = new Exporter(fileUtils);

            String tag = "backup";// reader.readString();
            //Console.Out.WriteLine(("Exporting to:" + exportFolder));

                exporter.writeJSONFile(exp_1, exp_5, exp_25, exp_100, exp_250, tag,1,backupDir);


            // end if type jpge or stack

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

            notifier.ShowInformation("Backing up CloudCoins Completed.");



            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One

        private void cmdBackup_Click(object sender, RoutedEventArgs e)
        {
            Banker bank = new Banker(MainWindow.fileUtils);
            int[] bankTotals = bank.countCoins(MainWindow.fileUtils.BankFolder);
            int[] frackedTotals = bank.countCoins(MainWindow.fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(MainWindow.fileUtils.partialFolder);


            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    export(dialog.SelectedPath);
                    //copyFolders(dialog.SelectedPath);
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
