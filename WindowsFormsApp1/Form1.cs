using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudCoinCore;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public static int timeout = 10000; // Milliseconds to wait until the request is ended. 
        public static String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static String importFolder = rootFolder + "Import" + Path.DirectorySeparatorChar;
        public static String importedFolder = rootFolder + "Imported" + 
            Path.DirectorySeparatorChar;
        public static String trashFolder = rootFolder + "Trash" + Path.DirectorySeparatorChar;
        public static String suspectFolder = rootFolder + "Suspect" + Path.DirectorySeparatorChar;
        public static String frackedFolder = rootFolder + "Fracked" + Path.DirectorySeparatorChar;
        public static String bankFolder = rootFolder + "Bank" + Path.DirectorySeparatorChar;
        public static String templateFolder = rootFolder + "Templates" + Path.DirectorySeparatorChar;
        public static String counterfeitFolder = rootFolder + "Counterfeit" + Path.DirectorySeparatorChar;
        public static String directoryFolder = rootFolder + "Directory" + Path.DirectorySeparatorChar;
        public static String exportFolder = rootFolder + "Export" + Path.DirectorySeparatorChar;
        public static String languageFolder = rootFolder + "Language" + Path.DirectorySeparatorChar;
        public static String partialFolder = rootFolder + "Partial" + Path.DirectorySeparatorChar;
        
        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };
        string dashline = "==================================================" + Environment.NewLine;

        public FileUtils fileUtils = new FileUtils(rootFolder, importFolder, importedFolder, trashFolder, suspectFolder, frackedFolder, bankFolder, templateFolder, counterfeitFolder, directoryFolder, exportFolder,partialFolder);

        public static Random myRandom = new Random();

        public Form1()
        {
            InitializeComponent();
            fileUtils.CreateDirectoryStructure();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            echoRaida();

        }

        public void detect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout,txtLogs);
            int[] detectionResults = detector.detectAll();
            //Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            //Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
            //                                                                             // And the bank and the fractured for total
            //Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            //Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            //showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
        }//end detect

        public void showCoins()
        {
            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            int[] partialTotals = bank.countCoins(partialFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //Output  " 12.3"
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Out.WriteLine("                                                                    ");
            txtLogs.AppendText(Environment.NewLine);
            Console.Out.WriteLine("    Total Coins in Bank:    " + string.Format("{0,8:N0}", (bankTotals[0] + frackedTotals[0] + partialTotals[0])) + "                                ");
            txtLogs.AppendText("    Total Coins in Bank:    " + string.Format("{0,8:N0}", (bankTotals[0] + frackedTotals[0] + partialTotals[0])) + "                                ");

            Console.Out.WriteLine("                                                                    ");
            txtLogs.AppendText("                                                                    ");
            Console.Out.WriteLine("                 1s         5s         25s       100s       250s    ");
            txtLogs.AppendText("                 1s         5s         25s       100s       250s    ");
            Console.Out.WriteLine("                                                                    ");
            txtLogs.AppendText(Environment.NewLine);

            Console.Out.WriteLine("   Perfect:   " + string.Format("{0,7}", bankTotals[1]) + "    " + string.Format("{0,7}", bankTotals[2]) + "    " + string.Format("{0,7}", bankTotals[3]) + "    " + string.Format("{0,7}", bankTotals[4]) + "    " + string.Format("{0,7}", bankTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            txtLogs.AppendText("   Perfect:   " + string.Format("{0,7}", bankTotals[1]) + "    " + string.Format("{0,7}", bankTotals[2]) + "    " + string.Format("{0,7}", bankTotals[3]) + "    " + string.Format("{0,7}", bankTotals[4]) + "    " + string.Format("{0,7}", bankTotals[5]) + "   "+Environment.NewLine);

            Console.Out.WriteLine("   Partial:   " + string.Format("{0,7}", partialTotals[1]) + "    " + string.Format("{0,7}", partialTotals[2]) + "    " + string.Format("{0,7}", partialTotals[3]) + "    " + string.Format("{0,7}", partialTotals[4]) + "    " + string.Format("{0,7}", partialTotals[5]) + "   ");

            txtLogs.AppendText("   Partial:   " + string.Format("{0,7}", partialTotals[1]) + "    " + string.Format("{0,7}", partialTotals[2]) + "    " + string.Format("{0,7}", partialTotals[3]) + "    " + string.Format("{0,7}", partialTotals[4]) + "    " + string.Format("{0,7}", partialTotals[5]) + "   "+ Environment.NewLine);

            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Fracked:   " + string.Format("{0,7}", frackedTotals[1]) + "    " + string.Format("{0,7}", frackedTotals[2]) + "    " + string.Format("{0,7}", frackedTotals[3]) + "    " + string.Format("{0,7}", frackedTotals[4]) + "    " + string.Format("{0,7}", frackedTotals[5]) + "   ");
            txtLogs.AppendText("   Fracked:   " + string.Format("{0,7}", frackedTotals[1]) + "    " + string.Format("{0,7}", frackedTotals[2]) + "    " + string.Format("{0,7}", frackedTotals[3]) + "    " + string.Format("{0,7}", frackedTotals[4]) + "    " + string.Format("{0,7}", frackedTotals[5]) + "   "+Environment.NewLine);
            txtLogs.AppendText(dashline);

            Console.Out.WriteLine("                                                                    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

        }// end show
        public bool echoRaida()
        {
            RAIDA_Status.resetEcho();
            RAIDA raida1 = new RAIDA(5000);
            Response[] results = raida1.echoAll(5000);
            int totalReady = 0;
            Console.Out.WriteLine("");
            txtLogs.AppendText(Environment.NewLine + dashline
                + Environment.NewLine + "Starting echo..." + Environment.NewLine + dashline);

            //For every RAIDA check its results
            int longestCountryName = 15;
            Console.Out.WriteLine();
            for (int i = 0; i < 25; i++)
            {
                int padding = longestCountryName - countries[i].Length;
                string strPad = "";
                for (int j = 0; j < padding; j++)
                {
                    strPad += " ";
                }//end for padding
                 // Console.Out.Write(RAIDA_Status.failsEcho[i]);
                if (RAIDA_Status.failsEcho[i])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.Write(strPad + countries[i]);
                    txtLogs.AppendText(strPad + countries[i]);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.Write(strPad + countries[i]);
                    MissingExtensions.AppendText(txtLogs, strPad + countries[i], Color.Green);

                    totalReady++;
                }
                if (i == 4 || i == 9 || i == 14 || i == 19)
                {
                    txtLogs.AppendText(Environment.NewLine);
                    Console.WriteLine();
                }
            }//end for
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            txtLogs.Text += "\n\n";
            txtLogs.AppendText(Environment.NewLine);
            Console.Out.Write("  RAIDA Health: " + totalReady + " / 25: ");//"RAIDA Health: " + totalReady );
            txtLogs.AppendText(dashline + "  RAIDA Health: " + RAIDA_Status.failsEcho.Where(c => !c).Count()
            +" / 25: ");



            //Check if enough are good 
            if (totalReady < 16)//
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  Not enough RAIDA servers can be contacted to import new coins.");// );
                Console.Out.WriteLine("  Is your device connected to the Internet?");// );
                Console.Out.WriteLine("  Is a router blocking your connection?");// );
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("The RAIDA is ready for counterfeit detection.");// );
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }//end if enough RAIDA
        }//End echo

        private void button1_Click(object sender, EventArgs e)
        {
            echoRaida();

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            import();
        }
        public static void showFolders()
        {
            Console.Out.WriteLine(" Your Root folder is:" + "\n " + rootFolder);
            Console.Out.WriteLine(" Your Import folder is:" + "\n  " + importFolder);
            Console.Out.WriteLine(" Your Imported folder is:" + "\n  " + importedFolder);
            Console.Out.WriteLine(" Your Suspect folder is: " + "\n  " + suspectFolder);
            Console.Out.WriteLine(" Your Trash folder is:" + "\n  " + trashFolder);
            Console.Out.WriteLine(" Your Bank folder is:" + "\n  " + bankFolder);
            Console.Out.WriteLine(" Your Fracked folder is:" + "\n  " + frackedFolder);
            Console.Out.WriteLine(" Your Templates folder is:" + "\n  " + templateFolder);
            Console.Out.WriteLine(" Your Directory folder is:" + "\n  " + directoryFolder);
            Console.Out.WriteLine(" Your Counterfeits folder is:" + "\n  " + counterfeitFolder);
            Console.Out.WriteLine(" Your Export folder is:" + "\n  " + exportFolder);
        } // end show folders

        private void cmdShowCoins_Click(object sender, EventArgs e)
        {
            showCoins();
        }

        public async void import()
        {
            txtLogs.Select(txtLogs.Text.Length - 1, 0);
            //Check RAIDA Status
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
                Console.Out.WriteLine("You do not have enought RAIDA to perform an import operation.");
                Console.Out.WriteLine("Check to make sure your internet is working.");
                Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                txtLogs.AppendText("You do not have enought RAIDA to perform an import operation." +Environment.NewLine);
                txtLogs.AppendText("Check to make sure your internet is working." + Environment.NewLine);
                txtLogs.AppendText("Make sure no routers at your work are blocking access to the RAIDA." + Environment.NewLine);
                txtLogs.AppendText("Try to Echo RAIDA and see if the status has changed."+Environment.NewLine);


                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                txtLogs.AppendText("  Finishing importing coins from last time...");
                Console.ForegroundColor = ConsoleColor.White;
                //detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");
                txtLogs.AppendText("  Now looking in import folder for new coins...");
                // "Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            txtLogs.AppendText(Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            txtLogs.AppendText("  Loading all CloudCoins in your import folder: "+ Environment.NewLine);
            Console.Out.WriteLine(importFolder);
            Console.ForegroundColor = ConsoleColor.White;
            Importer importer = new Importer(fileUtils,txtLogs);
            if (!importer.importAll())//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                txtLogs.AppendText("  No coins in import folder." + Environment.NewLine);
                Console.Out.WriteLine("  No coins in import folder.");// "No coins in import folder.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                detect();
            }//end if coins to import
        }   
    }
    public static class MissingExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            //box.SelectionFont = font;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

    }

}
