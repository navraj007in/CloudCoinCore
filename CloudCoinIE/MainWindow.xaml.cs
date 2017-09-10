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
using System.Windows.Navigation;
using System.IO;
using Founders;
using System.Threading;
using System.Diagnostics;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;

namespace CloudCoinIE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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

        public static int exportOnes = 0;
        public static int exportFives = 0;
        public static int exportTens = 0;
        public static int exportQtrs = 0;
        public static int exportHundreds = 0;
        public static int exportTwoFifties = 0;
        public static int exportJpegStack = 2;
        public static string exportTag = "";

        public static FileUtils fileUtils;
        public MainWindow()
        {
            setupFolders();
            InitializeComponent();
            export.RefreshCoins += new EventHandler(Refresh);
            import.RefreshCoins += new EventHandler(Refresh);

            fileUtils.CreateDirectoryStructure();
            watch();

            new Thread(delegate () {
                fix();
            }).Start();

        }

        FileSystemWatcher watcher;
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {

                //Copies file to another directory.
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

                notifier.ShowInformation("Import folder has new coins.");
            });
        }
        private void watch()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = fileUtils.ImportFolder;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void Refresh(object sender,  EventArgs e)
        {
            bank.showCoins();
            export.showCoins();
        }
        public void setupFolders()
        {
            rootFolder = getWorkspace();

            importFolder = rootFolder + "Import" + Path.DirectorySeparatorChar;
            importedFolder = rootFolder + "Imported" + Path.DirectorySeparatorChar;
            trashFolder = rootFolder + "Trash" + Path.DirectorySeparatorChar;
            suspectFolder = rootFolder + "Suspect" + Path.DirectorySeparatorChar;
            frackedFolder = rootFolder + "Fracked" + Path.DirectorySeparatorChar;
            bankFolder = rootFolder + "Bank" + Path.DirectorySeparatorChar;
            templateFolder = rootFolder + "Templates" + Path.DirectorySeparatorChar;
            counterfeitFolder = rootFolder + "Counterfeit" + Path.DirectorySeparatorChar;
            directoryFolder = rootFolder + "Directory" + Path.DirectorySeparatorChar;
            exportFolder = rootFolder + "Export" + Path.DirectorySeparatorChar;
            languageFolder = rootFolder + "Language" + Path.DirectorySeparatorChar;
            partialFolder = rootFolder + "Partial" + Path.DirectorySeparatorChar;

            fileUtils = FileUtils.GetInstance(MainWindow.rootFolder);


        }
        public string getWorkspace()
        {
            string workspace = "";
            if (Properties.Settings.Default.WorkSpace != null && Properties.Settings.Default.WorkSpace.Length > 0)
                workspace = Properties.Settings.Default.WorkSpace;
            else
                workspace = AppDomain.CurrentDomain.BaseDirectory;
            Properties.Settings.Default.WorkSpace = workspace;
                return workspace;
        }
        public void fix()
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
                Console.Out.WriteLine("You do not have enought RAIDA to perform a fix operation.");
                Console.Out.WriteLine("Check to make sure your internet is working.");
                Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            Console.Out.WriteLine("  Fixing fracked coins can take many minutes.");
            Console.Out.WriteLine("  If your coins are not completely fixed, fix fracked again.");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Attempting to fix all fracked coins.");
            Console.Out.WriteLine("");
            Frack_Fixer fixer = new Frack_Fixer(fileUtils, timeout);
            fixer.fixAll();
            stopwatch.Stop();
            Console.Out.WriteLine("  Fix Time: " + stopwatch.Elapsed + " ms");
            Console.Out.WriteLine("  If your coins are not completely fixed, you may 'fix fracked' again.");
        }//end fix

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            TextRange t = new TextRange(import.txtLogs.Document.ContentStart,
                                            import.txtLogs.Document.ContentEnd);
            FileStream file = new FileStream("Good File.xaml", FileMode.Open);
            t.Load(file, System.Windows.DataFormats.XamlPackage);
            file.Close();

        }
    }
}
    