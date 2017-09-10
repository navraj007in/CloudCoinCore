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
        public static int timeout = 10000; // Milliseconds to wait until the request is ended. 


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
        }

        private void cmdImport_Click(object sender, RoutedEventArgs e)
        {
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

                txtLogs.AppendText("You do not have enough RAIDA to perform an import operation.");
                txtLogs.AppendText("Check to make sure your internet is working.");
                txtLogs.AppendText("Make sure no routers at your work are blocking access to the RAIDA.");
                txtLogs.AppendText("Try to Echo RAIDA and see if the status has changed.");

                return;
            }
            progressBar.Visibility = Visibility.Visible;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                import();

                /* run your code here */
                Console.WriteLine("Hello, world");
            }).Start();
        }

        public void import()
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
            if (!importer.importAll())//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  No coins in import folder.");// "No coins in import folder.");
                updateLog("No coins in import Folder");

                Console.ForegroundColor = ConsoleColor.White;
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
            Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
            updateLog("  Total imported to bank: " + detectionResults[0]);
            updateLog("  Total imported to fracked: " + detectionResults[2]);
            // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            updateLog("  Total Counterfeit: " + detectionResults[1]);
            updateLog("  Total Kept in suspect folder: " + detectionResults[3]);

//            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
            updateLog(stopwatch.Elapsed + " ms");

            string messageBoxText = "Finished Importing Coins.";
            string caption = "Coins";
            RefreshCoins?.Invoke(this, new EventArgs());

            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBox.Show(messageBoxText, caption, button, icon);

        }//end detect

        private void Detector_OnUpdateStatus(object sender, CloudCoinInvestors.ProgressEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                progressBar.Value = e.percentage;
                if(e.percentage>0)
                    lblStatus.Content = String.Format("{0} % of Coins Scanned.", Convert.ToString(e.percentage));

            });
        }

        private void updateLog(string logLine)
        {
            App.Current.Dispatcher.Invoke(delegate {
                txtLogs.AppendText(logLine + Environment.NewLine);
            });

        }

        private void txtLogs_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtLogs.ScrollToEnd();
        }

        private void cmdRestore_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
