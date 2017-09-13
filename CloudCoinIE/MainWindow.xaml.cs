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
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
            showDisclaimer();
            setupFolders();
            InitializeComponent();
            export.RefreshCoins += new EventHandler(Refresh);
            import.RefreshCoins += new EventHandler(Refresh);

            fileUtils.CreateDirectoryStructure();
            watch();
            FileAssociations.SetAssociation("stack", "CloudCoinIE", "CloudCoin File", rootFolder + Path.DirectorySeparatorChar + "CloudCoinIE.exe");
            new Thread(delegate () {
                fix();
            }).Start();
            raida.cmdEcho_Click(this, new RoutedEventArgs());
        }

        private void showDisclaimer()
        {
            //Properties.Settings.Default["FirstRun"] = false;

            bool firstRun = (bool)Properties.Settings.Default["FirstRun"];
            if ( firstRun == false)
            {
                //First application run
                //Update setting
                Properties.Settings.Default["FirstRun"] = true;
                //Save setting
                Properties.Settings.Default.Save();

                Disclaimer disclaimer = new Disclaimer();
                disclaimer.ShowDialog();

                //Create new instance of Dialog you want to show
                //FirstDialogForm fdf = new FirstDialogForm();
                //Show the dialog
                //fdf.ShowDialog();
            }
            else
            {
                //Not first time of running application.
            }
        }


        FileSystemWatcher watcherImport;
        FileSystemWatcher watcherBank;

        private void OnChangedBank(object source, FileSystemEventArgs e)
        {
            bank.showCoins();
        }
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
            watcherImport = new FileSystemWatcher();
            watcherImport.Path = fileUtils.ImportFolder;
            watcherImport.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcherImport.Filter = "*.*";
            watcherImport.Changed += new FileSystemEventHandler(OnChanged);
            watcherImport.EnableRaisingEvents = true;

            watcherBank = new FileSystemWatcher();
            watcherBank.Path = fileUtils.BankFolder;
            watcherBank.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcherBank.Filter = "*.*";
            watcherBank.Changed += new FileSystemEventHandler(OnChangedBank);
            watcherBank.EnableRaisingEvents = true;

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

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // If data is dirty, notify user and ask for a response
            //if (!import.cmdImport.IsEnabled)
            //{
            //    string msg = "Import is in progress!";
            //    MessageBoxResult result =
            //      MessageBox.Show(
            //        msg,
            //        "CloudCoins",
            //        MessageBoxButton.OK ,
            //        MessageBoxImage.Warning);
            //    e.Cancel = true;
            //}

        }
    }
    public class FileAssociation
    {
        public string Extension { get; set; }
        public string ProgId { get; set; }
        public string FileTypeDescription { get; set; }
        public string ExecutableFilePath { get; set; }
    }

    public class FileAssociations
    {
        // needed so that Explorer windows get refreshed after the registry is updated
        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        private const int SHCNE_ASSOCCHANGED = 0x8000000;
        private const int SHCNF_FLUSH = 0x1000;

        public static void EnsureAssociationsSet()
        {
            var filePath = Process.GetCurrentProcess().MainModule.FileName;
            EnsureAssociationsSet(
                new FileAssociation
                {
                    Extension = ".ucs",
                    ProgId = "UCS_Editor_File",
                    FileTypeDescription = "UCS File",
                    ExecutableFilePath = filePath
                });
        }

        public static void EnsureAssociationsSet(params FileAssociation[] associations)
        {
            bool madeChanges = false;
            foreach (var association in associations)
            {
                madeChanges |= SetAssociation(
                    association.Extension,
                    association.ProgId,
                    association.FileTypeDescription,
                    association.ExecutableFilePath);
            }

            if (madeChanges)
            {
                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static bool SetAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
        {
            bool madeChanges = false;
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + extension, progId);
            madeChanges |= SetKeyDefaultValue(@"Software\Classes\" + progId, fileTypeDescription);
            madeChanges |= SetKeyDefaultValue($@"Software\Classes\{progId}\shell\open\command", "\"" + applicationFilePath + "\" \"%1\"");
            return madeChanges;
        }

        private static bool SetKeyDefaultValue(string keyPath, string value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
            {
                if (key.GetValue(null) as string != value)
                {
                    key.SetValue(null, value);
                    return true;
                }
            }

            return false;
        }
    }
    }
    