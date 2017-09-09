using Founders;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace CloudCoinIE.UserControls
{
    /// <summary>
    /// Interaction logic for Export.xaml
    /// </summary>
    public partial class Export : UserControl
    {
        int onesTotal = 0;
        int fivesTotal = 0;
        int qtrsTotal = 0;
        int hundredsTotal = 0;
        int TwoFiftiesTotal = 0;

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

        public static int exportOnes = 0;
        public static int exportFives = 0;
        public static int exportTens = 0;
        public static int exportQtrs = 0;
        public static int exportHundreds = 0;
        public static int exportTwoFifties = 0;
        public static int exportJpegStack = 2;
        public static string exportTag = "";

        public static FileUtils fileUtils = new FileUtils(rootFolder, importFolder, importedFolder, trashFolder, suspectFolder, frackedFolder, bankFolder, templateFolder, counterfeitFolder, directoryFolder, exportFolder, partialFolder);

        public Export()
        {
            InitializeComponent();
            showCoins();
        }
        public void showCoins()
        {
            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(MainWindow.fileUtils);
            int[] bankTotals = bank.countCoins(MainWindow.bankFolder);
            int[] frackedTotals = bank.countCoins(MainWindow.frackedFolder);
            int[] partialTotals = bank.countCoins(MainWindow.partialFolder);

            onesTotal = bankTotals[1] + frackedTotals[1] + partialTotals[1];
            fivesTotal = bankTotals[2] + frackedTotals[2] + partialTotals[2];
            qtrsTotal = bankTotals[3] + frackedTotals[3] + partialTotals[3];
            hundredsTotal = bankTotals[4] + frackedTotals[4] + partialTotals[4];
            TwoFiftiesTotal = bankTotals[5] + frackedTotals[5] + partialTotals[5];

            countOnes.Maximum = onesTotal;
            countFive.Maximum = fivesTotal;
            countQtrs.Maximum = qtrsTotal;
            countHundreds.Maximum = hundredsTotal;
            countTwoFifties.Maximum = TwoFiftiesTotal;

        }
        int total = 0;
        private void updateTotal()
        {
            try
            {
                total = Convert.ToInt16(countOnes.Value) + Convert.ToInt16(countFive.Value) * 5 + Convert.ToInt16(countQtrs.Value) * 25 + Convert.ToInt16(countHundreds.Value) * 100 + Convert.ToInt16(countTwoFifties.Value) * 250;
                if (total > 0)
                    groupExport.Header = "Export Your Coins - " + total;
                else
                    groupExport.Header = "Export Your Coins " ;

            }
            catch (Exception e)
            {

            }
        }
        private void countOnes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if(countOnes!=null && lblOnesValue!=null)
                lblOnesValue.Content = Convert.ToInt16(countOnes.Value);
            updateTotal();
        }

        private void countFive_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countFive != null && lblFivesValue != null)
                lblFivesValue.Content = Convert.ToInt16(countFive.Value) * 5;
            updateTotal();
        }

        private void countQtrs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countQtrs != null && lblQtrValue != null)
                lblQtrValue.Content = Convert.ToInt16(countQtrs.Value) * 25;
            updateTotal();
        }

        private void countHundreds_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countHundreds != null && lblHundredValue != null)
                lblHundredValue.Content = Convert.ToInt16(countHundreds.Value) * 100;
            updateTotal();
        }

        private void countTwoFifties_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countTwoFifties != null && lblTwoFiftiesValue != null)
                lblTwoFiftiesValue.Content = Convert.ToInt16(countTwoFifties.Value) * 250;
            updateTotal();
        }

        private void cmdExport_Click(object sender, RoutedEventArgs e)
        {
            export();
        }

        public void export()
        {
            if (rdbJpeg.IsChecked == true )
                exportJpegStack = 1;
            else
                exportJpegStack = 2;

            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            int[] partialTotals = bank.countCoins(partialFolder);

            //updateLog("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            showCoins();
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = Convert.ToInt16(countOnes.Value);
            int exp_5 = Convert.ToInt16(countFive.Value);
            int exp_25 = Convert.ToInt16(countQtrs.Value);
            int exp_100 = Convert.ToInt16(countHundreds.Value);
            int exp_250 = Convert.ToInt16(countTwoFifties.Value);
            //Warn if too many coins


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
            int file_type = 0; //reader.readInt(1, 2);





            Exporter exporter = new Exporter(fileUtils);
            exporter.OnUpdateStatus += Exporter_OnUpdateStatus; 
            file_type = exportJpegStack;

            String tag = exportTag;// reader.readString();
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

            notifier.ShowInformation("  Exporting CloudCoins Completed.");


            //updateLog("Exporting CloudCoins Completed.");
            showCoins();
            Process.Start(MainWindow.exportFolder);
            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One

        private void Exporter_OnUpdateStatus(object sender, CloudCoinInvestors.ProgressEventArgs e)
        {

        }
    }
}
