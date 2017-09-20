﻿using Founders;
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

        public Export()
        {
            InitializeComponent();
            calcMaximums();
            showCoins();
        }
        public void calcMaximums()
        {
            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(MainWindow.fileUtils);
            int[] bankTotals = bank.countCoins(MainWindow.fileUtils.BankFolder);
            int[] frackedTotals = bank.countCoins(MainWindow.fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(MainWindow.fileUtils.partialFolder);

            onesTotal = bankTotals[1] + frackedTotals[1] + partialTotals[1];
            fivesTotal = bankTotals[2] + frackedTotals[2] + partialTotals[2];
            qtrsTotal = bankTotals[3] + frackedTotals[3] + partialTotals[3];
            hundredsTotal = bankTotals[4] + frackedTotals[4] + partialTotals[4];
            TwoFiftiesTotal = bankTotals[5] + frackedTotals[5] + partialTotals[5];

            App.Current.Dispatcher.Invoke(delegate
            {

                countOnes.Maximum = onesTotal;
                countFive.Maximum = fivesTotal;
                countQtrs.Maximum = qtrsTotal;
                countHundreds.Maximum = hundredsTotal;
                countTwoFifties.Maximum = TwoFiftiesTotal;
                countOnes.Value = 0;
                countFive.Value = 0;
                countQtrs.Value = 0;
                countHundreds.Value = 0;
                countTwoFifties.Value = 0;
            });
        }
        int total = 0;
        private void updateTotal()
        {
            try
            {
                total = Convert.ToInt16(countOnes.Value) + Convert.ToInt16(countFive.Value) * 5 + Convert.ToInt16(countQtrs.Value) * 25 + Convert.ToInt16(countHundreds.Value) * 100 + Convert.ToInt16(countTwoFifties.Value) * 250;
                //if (total > 0)
                //    groupExport.Header = "Export Your Coins - " + total;
                //else
                //    groupExport.Header = "Export Your Coins " ;
                cmdExport.Content = "Export " + total + " Coins";
            }
            catch (Exception e)
            {

            }
        }

        public void showCoins()
        {
            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(MainWindow.fileUtils);
            int[] bankTotals = bank.countCoins(MainWindow.fileUtils.bankFolder);
            int[] frackedTotals = bank.countCoins(MainWindow.fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(MainWindow.fileUtils.partialFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //Output  " 12.3"
            onesTotal = bankTotals[1] + frackedTotals[1] + partialTotals[1];
            fivesTotal = bankTotals[2] + frackedTotals[2] + partialTotals[2];
            qtrsTotal = bankTotals[3] + frackedTotals[3] + partialTotals[3];
            hundredsTotal = bankTotals[4] + frackedTotals[4] + partialTotals[4];
            TwoFiftiesTotal = bankTotals[5] + frackedTotals[5] + partialTotals[5];


            setLabelText(lblOnesCount, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            setLabelText(lblFivesCount, Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2]));
            setLabelText(lblQtrCount, Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3]));
            setLabelText(lblHundredCount, Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4]));
            setLabelText(lblTwoFiftiesCount, Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5]));

            setLabelText(lblOnesValue, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            setLabelText(lblFivesValue, Convert.ToString((bankTotals[2] + frackedTotals[2] + partialTotals[2]) * 5));
            setLabelText(lblQtrValue, Convert.ToString((bankTotals[3] + frackedTotals[3] + partialTotals[3]) * 25));
            setLabelText(lblHundredValue, Convert.ToString((bankTotals[4] + frackedTotals[4] + partialTotals[4]) * 100));
            setLabelText(lblTwoFiftiesValue, Convert.ToString((bankTotals[5] + frackedTotals[5] + partialTotals[5]) * 250));

            App.Current.Dispatcher.Invoke(delegate
            {

                countOnes.Maximum = onesTotal;
                countFive.Maximum = fivesTotal;
                countQtrs.Maximum = qtrsTotal;
                countHundreds.Maximum = hundredsTotal;
                countTwoFifties.Maximum = TwoFiftiesTotal;
                countOnes.Value = 0;
                countFive.Value = 0;
                countQtrs.Value = 0;
                countHundreds.Value = 0;
                countTwoFifties.Value = 0;
            });

            //            setLabelText(lblTotalCoins, "Total Coins in Bank : " + Convert.ToString(bankTotals[0] + frackedTotals[0] + partialTotals[0]));
            //          setLabelText(lblValuesTotal, Convert.ToString(bankTotals[0] + frackedTotals[0] + partialTotals[0]));
            //        setLabelText(lblNotesTotal, Convert.ToString(
            //            Convert.ToInt16(lblOnesCount.Content.ToString()) +
            //           Convert.ToInt16(lblFivesCount.Content.ToString()) +
            //           Convert.ToInt16(lblQtrCount.Content.ToString()) +
            //         Convert.ToInt16(lblHundredCount.Content.ToString()) +
            //           Convert.ToInt16(lblTwoFiftiesCount.Content.ToString())
            //           ));


        }// end show

        private void countOnes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if(countOnes!=null && lblOnesExport!=null)
                lblOnesExport.Content = Convert.ToInt16(countOnes.Value);
            updateTotal();
        }
        private void setLabelText(Label lbl, string text)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                if (lbl != null)
                    lbl.Content = text;
            });

        }

        private void countFive_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countFive != null && lblFivesExport != null)
                lblFivesExport.Content = Convert.ToInt16(countFive.Value) * 5;
            updateTotal();
        }

        private void countQtrs_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countQtrs != null && lblQtrsExport != null)
                lblQtrsExport.Content = Convert.ToInt16(countQtrs.Value) * 25;
            updateTotal();
        }

        private void countHundreds_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countHundreds != null && lblHundredsExport != null)
                lblHundredsExport.Content = Convert.ToInt16(countHundreds.Value) * 100;
            updateTotal();
        }

        private void countTwoFifties_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (countTwoFifties != null && lblTwoFiftiesExport != null)
                lblTwoFiftiesExport.Content = Convert.ToInt16(countTwoFifties.Value) * 250;
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
            int[] bankTotals = bank.countCoins(fileUtils.BankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);

            //updateLog("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = Convert.ToInt16(countOnes.Value);
            int exp_5 = Convert.ToInt16(countFive.Value);
            int exp_25 = Convert.ToInt16(countQtrs.Value);
            int exp_100 = Convert.ToInt16(countHundreds.Value);
            int exp_250 = Convert.ToInt16(countTwoFifties.Value);
            //Warn if too many coins

            if(exp_1+ exp_5 + exp_25+ exp_100 + exp_250  == 0)
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
            int file_type = 0; //reader.readInt(1, 2);





            Exporter exporter = new Exporter(fileUtils);
            exporter.OnUpdateStatus += Exporter_OnUpdateStatus; 
            file_type = exportJpegStack;

            String tag = txtTag.Text;// reader.readString();
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

            notifier.ShowInformation("Exporting CloudCoins Completed.");

            RefreshCoins?.Invoke(this, new EventArgs());
            //updateLog("Exporting CloudCoins Completed.");
            showCoins();
            Process.Start(fileUtils.exportFolder);
            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One

        public void setLimits()
        {

        }
        private void Exporter_OnUpdateStatus(object sender, CloudCoinInvestors.ProgressEventArgs e)
        {

        }
    }
}
