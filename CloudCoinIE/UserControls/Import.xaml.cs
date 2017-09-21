using Founders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;

using System.IO;
using System.Threading;
using System.Windows.Interactivity;
using Microsoft.Win32;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using System.Windows.Threading;

namespace CloudCoinIE.UserControls
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    /// 
    public class ScrollToBottomAction : TriggerAction<RichTextBox>
    {
        protected override void Invoke(object parameter)
        {
            AssociatedObject.ScrollToEnd();
        }
    }
    public partial class Import : UserControl
    {
        public static int timeout = 5000; // Milliseconds to wait until the request is ended. 


        public static int exportOnes = 0;
        public static int exportFives = 0;
        public static int exportTens = 0;
        public static int exportQtrs = 0;
        public static int exportHundreds = 0;
        public static int exportTwoFifties = 0;
        public static int exportJpegStack = 2;
        public static string exportTag = "";

        public static FileUtils fileUtils = FileUtils.GetInstance(MainWindow.rootFolder);
        public EventHandler RefreshCoins;


        public Import()
        {
            InitializeComponent();
            Dispatcher.BeginInvoke(new Action(() => resumeImport()), 
                DispatcherPriority.ContextIdle, null);
            //resumeImport();
        }

        private void resumeImport()
        {

            int count = Directory.GetFiles(MainWindow.suspectFolder ).Length;
            if(count >0 )
            {
                cmdImport.IsEnabled = false;
                cmdRestore.IsEnabled = false;
                progressBar.Visibility = Visibility.Visible;

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

                notifier.ShowInformation("Unimported Coins found. Resuming import operation.");

                new Thread(() =>
                {

                    Thread.CurrentThread.IsBackground = true;

                    MainWindow.echoRaida();

                    int totalRAIDABad = 0;
                    for (int i = 0; i < 25; i++)
                    {
                        if (RAIDA_Status.failsEcho[i])
                        {
                            totalRAIDABad += 1;
                        }
                    }
                    if (totalRAIDABad > 8)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("You do not have enough RAIDA to perform an import operation.");
                        Console.Out.WriteLine("Check to make sure your internet is working.");
                        Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                        Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                        Console.ForegroundColor = ConsoleColor.White;

                        insufficientRAIDA();
                        return;
                    }
                    else
                        import();

                    /* run your code here */
                }).Start();

            }
        }
        private void cmdImport_Click(object sender, RoutedEventArgs e)
        {
            int count = Directory.GetFiles(MainWindow.importFolder).Length;
            if (count == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Cloudcoins (*.stack, *.jpg,*.jpeg)|*.stack;*.jpg;*.jpeg|Stack files (*.stack)|*.stack|Jpeg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = fileUtils.ImportFolder;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string filename in openFileDialog.FileNames)
                    {
                        try
                        {
                            if (!File.Exists(fileUtils.ImportFolder + Path.DirectorySeparatorChar + Path.GetFileName(filename)))
                                File.Move(filename, fileUtils.ImportFolder + Path.DirectorySeparatorChar + Path.GetFileName(filename));
                            else
                            {
                                string msg = "File " + filename + " already exists. Do you want to overwrite it?";
                                MessageBoxResult result =
                                  MessageBox.Show(
                                    msg,
                                    "CloudCoins",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Warning);
                                if (result == MessageBoxResult.Yes)
                                {
                                    try
                                    {
                                        File.Delete(fileUtils.ImportFolder + Path.DirectorySeparatorChar + Path.GetFileName(filename));
                                        File.Move(filename, fileUtils.ImportFolder + Path.DirectorySeparatorChar + Path.GetFileName(filename));
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            updateLog(ex.Message);
                        }
                    }
                }
                else
                    return;
            }

            int totalRAIDABad = 0;
            for (int i = 0; i < 25; i++)
            {
                if (RAIDA_Status.failsEcho[i])
                {
                    totalRAIDABad += 1;
                }
            }
            if (totalRAIDABad > 8)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("You do not have enough RAIDA to perform an import operation.");
                Console.Out.WriteLine("Check to make sure your internet is working.");
                Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                Console.ForegroundColor = ConsoleColor.White;

                insufficientRAIDA();

                return;
            }
            cmdImport.IsEnabled = false;
            cmdRestore.IsEnabled = false;
            progressBar.Visibility = Visibility.Visible;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                import();

                /* run your code here */
            }).Start();
        }

        public void import(int resume = 0)
        {

            //Check RAIDA Status

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(MainWindow.fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                updateLog("  Finishing importing coins from last time...");

                Console.ForegroundColor = ConsoleColor.White;
                detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
                updateLog("  Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(MainWindow.fileUtils.importFolder);
            updateLog("  Loading all CloudCoins in your import folder: ");
            updateLog(MainWindow.fileUtils.importFolder);

            Console.ForegroundColor = ConsoleColor.White;
            Importer importer = new Importer(fileUtils);
            if (!importer.importAll() && resume == 0)//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  No coins in import folder.");// "No coins in import folder.");
                updateLog("No coins in import Folder");

                Console.ForegroundColor = ConsoleColor.White;
                App.Current.Dispatcher.Invoke(delegate
                {

                    cmdRestore.IsEnabled = true;
                    cmdImport.IsEnabled = true;
                });

            }
            else
            {
                detect();
            }//end if coins to import
        }   // end import

        public void detect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            updateLog("  Detecting Authentication of Suspect Coins");

            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout);

            detector.OnUpdateStatus += Detector_OnUpdateStatus; ;
            detector.txtLogs = txtLogs;
            int[] detectionResults = detector.detectAll();
            Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            //Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
            updateLog("  Total imported to bank: " + detectionResults[0]);
            //updateLog("  Total imported to fracked: " + detectionResults[2]);
            // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            updateLog("  Total Counterfeit: " + detectionResults[1]);
            updateLog("  Total Kept in suspect folder: " + detectionResults[3]);
            updateLog("  Total Notes imported to Bank: " + detector.totalImported);

            //            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
            updateLog("Time to import "+ detectionResults[0] +" Coins: "+ stopwatch.Elapsed.ToCustomString() + "");

            string messageBoxText = "Finished Importing Coins.";
            string caption = "Coins";
            RefreshCoins?.Invoke(this, new EventArgs());

            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, icon);
            App.Current.Dispatcher.Invoke(delegate
            {
                cmdRestore.IsEnabled = true;
                cmdImport.IsEnabled = true;
                progressBar.Value = 100;
            });
        }//end detect

        private void Detector_OnUpdateStatus(object sender, CloudCoinInvestors.ProgressEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                progressBar.Value = e.percentage;
                if (e.percentage > 0)
                    lblStatus.Content = String.Format("{0} % of Coins Scanned.", Convert.ToString(e.percentage));

            });
        }

        private void insufficientRAIDA()
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                txtLogs.AppendText("You do not have enough RAIDA to perform an import operation.");
                txtLogs.AppendText("Check to make sure your internet is working.");
                txtLogs.AppendText("Make sure no routers at your work are blocking access to the RAIDA.");
                txtLogs.AppendText("Try to Echo RAIDA and see if the status has changed.");

                cmdImport.IsEnabled = true;
                cmdRestore.IsEnabled = true;
            });

        }
        private void updateLog(string logLine)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                txtLogs.AppendText(logLine + Environment.NewLine);
            });

        }

        private void txtLogs_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLogs.ScrollToEnd();
        }

        private void cmdRestore_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string restorePath = fileUtils.suspectFolder;
                    String SourcePath = dialog.SelectedPath + Path.DirectorySeparatorChar + "Bank";
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
    SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(SourcePath, restorePath), true);

                    SourcePath = dialog.SelectedPath + Path.DirectorySeparatorChar + "Fracked";
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
    SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(SourcePath, restorePath), true);
                    cmdImport_Click(this, new RoutedEventArgs());
                }
            }
        }
    }
    public static class MyExtensions
    {
        public static string ToCustomString(this TimeSpan span)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", span.Hours, span.Minutes, span.Seconds);
        }
    }
}
