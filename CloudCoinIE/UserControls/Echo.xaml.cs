using System;
using System.Collections.Generic;
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
using Founders;

namespace CloudCoinIE.UserControls
{
    /// <summary>
    /// Interaction logic for Echo.xaml
    /// </summary>
    public partial class Echo : UserControl
    {
        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };
        Button[] raidas = new Button[25];

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


        public Echo()
        {
            InitializeComponent();
            raidas[0] = lblAustralia;
            raidas[1] = lblMacedonia;
            raidas[2] = lblPhillipines;
            raidas[3] = lblSerbia;
            raidas[4] = lblBulgaria;
            raidas[5] = lblRussia;
            raidas[6] = lblSwitzerland;
            raidas[7] = lblUK;
            raidas[8] = lblPunjab;
            raidas[9] = lblIndia;
            raidas[10] = lblCroatia;
            raidas[11] = lblUSA;
            raidas[12] = lblIndia2;
            raidas[13] = lblTaiwan;
            raidas[14] = lblMoscow;
            raidas[15] = lblStPetersburg;
            raidas[16] = lblColumbia;
            raidas[17] = lblSingapore;
            raidas[18] = lblGermany;
            raidas[19] = lblCanada;
            raidas[20] = lblVenezuela;
            raidas[21] = lblHyderabad;
            raidas[22] = lblUSA2;
            raidas[23] = lblUkraine;
            raidas[24] = lblLuxemberg;

        }

        public bool echoRaida()
        {

            cmdRefresh.IsEnabled = false;

            RAIDA_Status.resetEcho();
            RAIDA raida1 = new RAIDA(5000);
            Response[] results = raida1.echoAll(5000);
            int totalReady = 0;
            Console.Out.WriteLine("");
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
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Out.Write(strPad + countries[i]);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Out.Write(strPad + countries[i]);
                    totalReady++;
                }
                if (RAIDA_Status.failsEcho[i])
                    raidas[i].Background = Brushes.Red;
                else
                    raidas[i].Background = Brushes.Green;

                if (i == 4 || i == 9 || i == 14 || i == 19) { Console.WriteLine(); }
            }//end for
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.Write("  RAIDA Health: " + totalReady + " / 25: ");//"RAIDA Health: " + totalReady );
            //lblHealth.Text = "  RAIDA Health: " + totalReady + " / 25: ";

                // Running on the UI thread
                lblHealth.Content = "  RAIDA Health: " + totalReady + " / 25: "; ;
                cmdRefresh.IsEnabled = true;

            //Check if enough are good 
            if (totalReady < 16)//
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  Not enough RAIDA servers can be contacted to import new coins.");// );
                Console.Out.WriteLine("  Is your device connected to the Internet?");// );
                Console.Out.WriteLine("  Is a router blocking your connection?");// );
                Console.ForegroundColor = ConsoleColor.White;
                lblHealth.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("The RAIDA is ready for counterfeit detection.");// );
                lblHealth.Foreground = Brushes.Green;
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }//end if enough RAIDA
        }//End echo


        private void cmdEcho_Click(object sender, RoutedEventArgs e)
        {
            echoRaida();
        }
    }
}
