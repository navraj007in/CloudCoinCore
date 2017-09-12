using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace CloudCoinIE
{
    /// <summary>
    /// Interaction logic for Disclaimer.xaml
    /// </summary>
    public partial class Disclaimer : MetroWindow
    {
        public Disclaimer()
        {
            InitializeComponent();
        }

        private void cmdAgree_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmdDisagree_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
