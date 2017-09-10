using Founders;
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
using System.Windows.Shapes;

namespace CloudCoinIE.UserControls
{
    /// <summary>
    /// Interaction logic for Bank.xaml
    /// </summary>
    public partial class Bank : UserControl
    {
        public class Item
        {
            
        }
        public Bank()
        {
            InitializeComponent();
            showCoins();
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

            
            setLabelText(lblOnesCount, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            setLabelText(lblFivesCount, Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2]));
            setLabelText(lblQtrCount, Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3]));
            setLabelText(lblHundredCount, Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4]));
            setLabelText(lblTwoFiftiesCount, Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5]));

            setLabelText(lblOnesValue, Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            setLabelText(lblFivesValue, Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2] * 5));
            setLabelText(lblQtrValue, Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3] * 25));
            setLabelText(lblHundredValue, Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4] * 100));
            setLabelText(lblTwoFiftiesValue, Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5] * 250));
            setLabelText(lblTotalCoins, "Total Coins : " + Convert.ToString(bankTotals[0] + frackedTotals[0] + partialTotals[0]));

        }// end show

        private void setLabelText(Label lbl, string text)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                if(lbl!=null)
                    lbl.Content = text;
            });

        }

    }
}
