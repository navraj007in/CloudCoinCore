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
            public string StaticColumnA { get; } = "Some static value";
            public string StaticColumnB { get; } = "Some static value";
            public string DynamicColumnA { get; set; }
        }
        public Bank()
        {
            InitializeComponent();
            List<Item> theItems = new List<Item>() { };
            dg.ItemsSource = theItems;
        }
    }
}
