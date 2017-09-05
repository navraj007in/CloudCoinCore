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
    public partial class frmExport : Form
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
        public frmCloudCoin cloudcoin;

        public frmExport()
        {
            InitializeComponent();
        }

        private void txtOnes_TextChanged(object sender, EventArgs e)
        {
            try { 
            if (Convert.ToInt16(txtOnes.Text) > Convert.ToInt16(lbl1Count.Text))
                txtOnes.Text = lbl1Count.Text;
            }
            catch (Exception ex)
            {
                txtOnes.Text = "0";
            }
            calculateTotal();
        }



        private void txtOnes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt5s_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt25s_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt100s_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt250s_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmExport_Load(object sender, EventArgs e)
        {
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            int[] partialTotals = bank.countCoins(partialFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //Output  " 12.3"
            lbl1Count.Text = Convert.ToString(bankTotals[1] + frackedTotals[1] + partialTotals[1]);
            lbl5Count.Text = Convert.ToString(bankTotals[2] + frackedTotals[2] + partialTotals[2]);
            lbl25Count.Text = Convert.ToString(bankTotals[3] + frackedTotals[3] + partialTotals[3]);
            lbl100Count.Text = Convert.ToString(bankTotals[4] + frackedTotals[4] + partialTotals[4]);
            lbl250Count.Text = Convert.ToString(bankTotals[5] + frackedTotals[5] + partialTotals[5]);


        }

        private void txt5s_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt16(txt5s.Text) > Convert.ToInt16(lbl5Count.Text))
                    txt5s.Text = lbl5Count.Text;
            }
            catch (Exception ex)
            {
                txt5s.Text = "0";
            }
            calculateTotal();

        }

        private void txt25s_TextChanged(object sender, EventArgs e)
        {
            try { 
            if (Convert.ToInt16(txt25s.Text) > Convert.ToInt16(lbl25Count.Text))
                txt25s.Text = lbl25Count.Text;
            }
            catch (Exception ex)
            {
                txt25s.Text = "0";
            }
            calculateTotal();

        }

        private void txt100s_TextChanged(object sender, EventArgs e)
        {
            try { 
            if (Convert.ToInt16(txt100s.Text) > Convert.ToInt16(lbl100Count.Text))
                txt100s.Text = lbl100Count.Text;
            }
            catch (Exception ex)
            {
                txt100s.Text = "0";
            }
            calculateTotal();

        }

        private void txt250s_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt16(txt250s.Text) > Convert.ToInt16(lbl250Count.Text))
                    txt250s.Text = lbl250Count.Text;
            }
            catch(Exception ex)
            {
                txt250s.Text = "0";
            }
            calculateTotal();
        }

        private void calculateTotal()
        {
            int count = 0;
            count += Convert.ToInt16(txtOnes.Text);
            count += Convert.ToInt16(txt5s.Text)*5;
            count += Convert.ToInt16(txt25s.Text)*25;
            count += Convert.ToInt16(txt100s.Text)*100;
            count += Convert.ToInt16(txt250s.Text)*250;

            lblTotalExportAmount.Text = "Total Export Amount : " + count;
        }
        private void cmdExport_Click(object sender, EventArgs e)
        {
            frmCloudCoin.exportOnes = Convert.ToInt16(txtOnes.Text);
            frmCloudCoin.exportFives = Convert.ToInt16(txt5s.Text);
            frmCloudCoin.exportQtrs = Convert.ToInt16(txt25s.Text);
            frmCloudCoin.exportHundreds = Convert.ToInt16(txt100s.Text);
            frmCloudCoin.exportTwoFifties = Convert.ToInt16(txt250s.Text);
            if (rdbJpeg.Checked)
                frmCloudCoin.exportJpegStack = 1;
            else
                frmCloudCoin.exportJpegStack = 2;

            this.Close();
            cloudcoin.export();

        }
    }
}
