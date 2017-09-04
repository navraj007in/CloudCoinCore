using Founders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloudCoinInvestors
{
    public partial class frmCloudCoin : Form
    {
        public static int timeout = 10000; // Milliseconds to wait until the request is ended. 

        public static String rootFolder = AppDomain.CurrentDomain.BaseDirectory;
        public static String importFolder = rootFolder + "Import" + Path.DirectorySeparatorChar;
        public static String importedFolder = rootFolder + "Imported" + Path.DirectorySeparatorChar;
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

        public static FileUtils fileUtils = new FileUtils(rootFolder, importFolder, importedFolder, trashFolder, suspectFolder, frackedFolder, bankFolder, templateFolder, counterfeitFolder, directoryFolder, exportFolder, partialFolder);

        public frmCloudCoin()
        {
            fileUtils.CreateDirectoryStructure();
            InitializeComponent();
        }

        private void cmdEcho_Click(object sender, EventArgs e)
        {
            frmEcho echo = new frmEcho();
            echo.Show();

        }

        private void showCoinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showCoins();
        }

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
            Console.Out.WriteLine("    Total Coins in Bank:    " + string.Format("{0,8:N0}", (bankTotals[0] + frackedTotals[0] + partialTotals[0])) + "                                ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("                 1s         5s         25s       100s       250s    ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Perfect:   " + string.Format("{0,7}", bankTotals[1]) + "    " + string.Format("{0,7}", bankTotals[2]) + "    " + string.Format("{0,7}", bankTotals[3]) + "    " + string.Format("{0,7}", bankTotals[4]) + "    " + string.Format("{0,7}", bankTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Partial:   " + string.Format("{0,7}", partialTotals[1]) + "    " + string.Format("{0,7}", partialTotals[2]) + "    " + string.Format("{0,7}", partialTotals[3]) + "    " + string.Format("{0,7}", partialTotals[4]) + "    " + string.Format("{0,7}", partialTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Fracked:   " + string.Format("{0,7}", frackedTotals[1]) + "    " + string.Format("{0,7}", frackedTotals[2]) + "    " + string.Format("{0,7}", frackedTotals[3]) + "    " + string.Format("{0,7}", frackedTotals[4]) + "    " + string.Format("{0,7}", frackedTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            lblOneCount.Text = Convert.ToString(bankTotals[1] + frackedTotals[1]+ partialTotals[1]);
            lblFiveCount.Text = Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2]);
            lblQtrCount.Text = Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3]);
            lblHundredCount.Text = Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4]);
            lbl250Count.Text = Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5]);

            lblOneTotal.Text = (lblOneCount.Text);
            lblFiveTotal.Text = Convert.ToString(Convert.ToInt16(lblFiveCount.Text) * 5);
            lblQtrTotal.Text = Convert.ToString(Convert.ToInt16(lblQtrCount.Text) * 25);
            lblHundredTotal.Text = Convert.ToString(Convert.ToInt16(lblHundredCount.Text) * 100);
            lbl250Total.Text = Convert.ToString(Convert.ToInt16(lbl250Total.Text) * 250);
            lblTotalCoins.Text = "Total Coins : " + Convert.ToString(bankTotals[0] + frackedTotals[0] + partialTotals[0]);
        }// end show

        public void showFolders()
        {
            updateLog(" Your Root folder is:" + "\n " + rootFolder);
            updateLog(" Your Import folder is:" + "\n  " + importFolder);
            updateLog(" Your Imported folder is:" + "\n  " + importedFolder);
            updateLog(" Your Suspect folder is: " + "\n  " + suspectFolder);
            updateLog(" Your Trash folder is:" + "\n  " + trashFolder);
            updateLog(" Your Bank folder is:" + "\n  " + bankFolder);
            updateLog(" Your Fracked folder is:" + "\n  " + frackedFolder);
            updateLog(" Your Templates folder is:" + "\n  " + templateFolder);
            updateLog(" Your Directory folder is:" + "\n  " + directoryFolder);
            updateLog(" Your Counterfeits folder is:" + "\n  " + counterfeitFolder);
            updateLog(" Your Export folder is:" + "\n  " + exportFolder);

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

        public void import()
        {

            //Check RAIDA Status

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
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
            Console.Out.WriteLine(importFolder);
            updateLog("  Loading all CloudCoins in your import folder: ");
            updateLog(importFolder);

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

        private void updateLog(string logLine)
        {
            txtLogs.Invoke((MethodInvoker)delegate
            {
                txtLogs.AppendText(logLine + Environment.NewLine);
                txtLogs.SelectionStart = txtLogs.TextLength;
                txtLogs.SelectionLength = 0;
                txtLogs.ScrollToCaret();
                // Running on the UI thread
            });
            }
        private void frmCloudCoin_Load(object sender, EventArgs e)
        {
            showCoins();
        }

        private void cmdImport_Click(object sender, EventArgs e)
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

            importWorker.DoWork += ImportWorker_DoWork;
            importWorker.RunWorkerAsync();
        }

        private void ImportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            import();
        }

        private void showFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showFolders();
        }

        public void detect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            updateLog("  Detecting Authentication of Suspect Coins");

            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout);
            detector.frmCloudCoin = this;
            detector.OnUpdateStatus += Detector_OnUpdateStatus;
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

            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
            updateLog(stopwatch.Elapsed + " ms");

        }//end detect

        private void Detector_OnUpdateStatus(object sender, ProgressEventArgs e)
        {
            updateLog(e.Status);
        }

        public void export()
        {
            Console.Out.WriteLine("");
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            int[] partialTotals = bank.countCoins(partialFolder);
            Console.Out.WriteLine("  Your Bank Inventory:");
            updateLog("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            showCoins();
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = 0;
            int exp_5 = 0;
            int exp_25 = 0;
            int exp_100 = 0;
            int exp_250 = 0;
            //Warn if too many coins
            Console.WriteLine(bankTotals[1] + frackedTotals[1] + bankTotals[2] + frackedTotals[2] + bankTotals[3] + frackedTotals[3] + bankTotals[4] + frackedTotals[4] + bankTotals[5] + frackedTotals[5] + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]);
            if (((bankTotals[1] + frackedTotals[1]) + (bankTotals[2] + frackedTotals[2]) + (bankTotals[3] + frackedTotals[3]) + (bankTotals[4] + frackedTotals[4]) + (bankTotals[5] + frackedTotals[5]) + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]) > 1000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                Console.Out.WriteLine("Do not export stack files with more than 1000 notes. .");
                Console.ForegroundColor = ConsoleColor.White;
            }//end if they have more than 1000 coins

            Console.Out.WriteLine("  Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?");
            int file_type = 0; //reader.readInt(1, 2);
            // 1 jpg 2 stack
            if ((bankTotals[1] + frackedTotals[1]) > 0)
            {
                Console.Out.WriteLine("  How many 1s do you want to export?");
                exp_1 = 0;// reader.readInt(0, (bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            }

            // if 1s not zero 
            if ((bankTotals[2] + frackedTotals[2]) > 0)
            {
                Console.Out.WriteLine("  How many 5s do you want to export?");
                exp_5 = 0;// reader.readInt(0, (bankTotals[2] + frackedTotals[2] + partialTotals[2]));
            }

            // if 1s not zero 
            if ((bankTotals[3] + frackedTotals[3] > 0))
            {
                Console.Out.WriteLine("  How many 25s do you want to export?");
                exp_25 = 0; //reader.readInt(0, (bankTotals[3] + frackedTotals[3] + partialTotals[3]));
            }

            // if 1s not zero 
            if ((bankTotals[4] + frackedTotals[4]) > 0)
            {
                Console.Out.WriteLine("  How many 100s do you want to export?");
                exp_100 = 0;// reader.readInt(0, (bankTotals[4] + frackedTotals[4] + partialTotals[4]));
            }

            // if 1s not zero 
            if ((bankTotals[5] + frackedTotals[5]) > 0)
            {
                Console.Out.WriteLine("  How many 250s do you want to export?");
                exp_250 = 0;// reader.readInt(0, (bankTotals[5] + frackedTotals[5] + partialTotals[5]));
            }

            // if 1s not zero 
            // move to export
            Exporter exporter = new Exporter(fileUtils);
            if (file_type == 1)
            {
                Console.Out.WriteLine("  Tag your jpegs with 'random' to give them a random number.");
                
            }
            Console.Out.WriteLine("  What tag will you add to the file name?");
            String tag = "";// reader.readString();
            //Console.Out.WriteLine(("Exporting to:" + exportFolder));
            if (file_type == 1)
            {
                exporter.writeJPEGFiles(exp_1, exp_5, exp_25, exp_100, exp_250, tag);
                // stringToFile( json, "test.txt");
            }
            else
            {
                exporter.writeJSONFile(exp_1, exp_5, exp_25, exp_100, exp_250, tag);
            }

            // end if type jpge or stack
            Console.Out.WriteLine("  Exporting CloudCoins Completed.");
            updateLog("  Exporting CloudCoins Completed.");
        }// end export One

        private void cmdExport_Click(object sender, EventArgs e)
        {
            frmExport export = new frmExport();
            export.ShowDialog();

        }
    }
}
