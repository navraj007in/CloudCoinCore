using Founders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public static void import()
        {

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
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                Console.ForegroundColor = ConsoleColor.White;
               // detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(importFolder);
            Console.ForegroundColor = ConsoleColor.White;
            Importer importer = new Importer(fileUtils);
            if (!importer.importAll())//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  No coins in import folder.");// "No coins in import folder.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
//                detect();
            }//end if coins to import
        }   // end import

        private void frmCloudCoin_Load(object sender, EventArgs e)
        {
            showCoins();
        }
    }
}
