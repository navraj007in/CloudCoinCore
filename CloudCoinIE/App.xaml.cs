using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CloudCoinIE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }

        private void showDisclaimer()
        {
            Disclaimer disclaimer = new Disclaimer();

            disclaimer.ShowDialog();

            //if ((bool)Properties.Settings.Default["FirstRun"] == false)
            //{
            //    //First application run
            //    //Update setting
            //    Properties.Settings.Default["FirstRun"] = true;
            //    //Save setting
            //    Properties.Settings.Default.Save();

            //    string messageBoxText = "CloudCoin Investor's Edition. V 9/11/2017. This software is provided as is with all faults, defects and errors, and without and warranty of any kind. Free from the CloudCoin Consortium";
            //    string caption = "CloudCoin";
            //    MessageBoxButton button = MessageBoxButton.OK;
            //    MessageBoxImage icon = MessageBoxImage.Information;

            //    MessageBox.Show(messageBoxText, caption, button, icon);

            //    //Create new instance of Dialog you want to show
            //    //FirstDialogForm fdf = new FirstDialogForm();
            //    //Show the dialog
            //    //fdf.ShowDialog();
            //}
            //else
            //{
            //    //Not first time of running application.
            //}
        }

    }
}
