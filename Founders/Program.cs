using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
//using Newtonsoft.Json;
using System.Xml;

namespace Founders
{
    class Program
    {
        /* INSTANCE VARIABLES */
        public static KeyboardReader reader = new KeyboardReader();
        //  public static String rootFolder = System.getProperty("user.dir") + File.separator +"bank" + File.separator ;
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
        public static String detectedFolder = rootFolder + "Detected" + Path.DirectorySeparatorChar;
        public static String receiptsFolder = rootFolder + "Reciepts" + Path.DirectorySeparatorChar;
        public static String dangerFolder = rootFolder + "Danger" + Path.DirectorySeparatorChar;
        public static String lostFolder = rootFolder + "Lost" + Path.DirectorySeparatorChar;




        public static String prompt = "> ";
        public static String[] commandsAvailable = new String[] { "Echo raida", "Show CloudCoins in Bank", "Import / Pown & Deposit", "Export / Withdraw", "Fix Fracked", "Show Folders", "Export stack files with one note each", "Help", "Quit" };
        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };

        //{ "echo raida", "show coins", "import", "export", "fix fracked", "show folders", "export for sales", "quit" };
        public static int timeout = 10000; // Milliseconds to wait until the request is ended. 
        public static FileUtils fileUtils = new FileUtils(rootFolder, importFolder, importedFolder, trashFolder, suspectFolder, frackedFolder, bankFolder, templateFolder, counterfeitFolder, directoryFolder, exportFolder, partialFolder, detectedFolder, receiptsFolder, dangerFolder, lostFolder);
        public static Random myRandom = new Random();


        /* MAIN METHOD */
        static void Main(string[] args)
        {

            fileUtils.CreateDirectoryStructure();
            int argLength = args.Length;
            if (argLength > 0)
            {
                handleCommand(args);
            }
            else
            {
                printWelcome();
                run();
            }
        }

        /* STATIC METHODS */
        public static void handleCommand(string[] args)
        {
            string command = args[0];

            switch (command)
            {
                case "echo":
                    echoRaida();
                    break;
                case "showcoins":
                    showCoins();
                    break;
                case "import":
                    import();
                    break;
                case "export":
                    export();
                    break;
                case "showfolders":
                    showFolders();
                    break;
                case "fix":
                    fix(timeout);
                    break;
                case "dump":
                    dump();
                    break;
                case "help":
                    help();
                    break;
                default:
                    break;
            }
        }
        public static void run()
        {
            bool restart = false;
            while (!restart)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("");
                //  Console.Out.WriteLine("========================================");
                Console.Out.WriteLine("");
                Console.Out.WriteLine("  Commands Available:");//"Commands Available:";
                Console.ForegroundColor = ConsoleColor.White;
                int commandCounter = 1;
                foreach (String command in commandsAvailable)
                {
                    Console.Out.WriteLine("  " + commandCounter + (". " + command));
                    commandCounter++;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.Write(prompt);
                Console.ForegroundColor = ConsoleColor.White;
                int commandRecieved = reader.readInt(1, 9);
                switch (commandRecieved)
                {
                    case 1:
                        echoRaida();
                        break;
                    case 2:
                        showCoins();
                        break;
                    case 3:
                        import();
                        break;
                    case 4:
                        export();
                        break;
                    case 5:
                        fix(timeout);
                        break;
                    case 6:
                        showFolders();
                        break;
                    case 7:
                        dump();
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    case 9:
                        //testMind();
                        //partialImport();
                        break;
                    case 10:
                        toMind();
                        break;
                    case 11:
                        fromMind();
                        break;
                    default:
                        Console.Out.WriteLine("Command failed. Try again.");//"Command failed. Try again.";
                        break;
                }// end switch
            }// end while
        }// end run method


        public static void printWelcome()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("                                                                  ");
            Console.Out.WriteLine("                   CloudCoin Founders Edition                     ");
            Console.Out.WriteLine("                      Version: October.10.2017                    ");
            Console.Out.WriteLine("          Used to Authenticate, Store and Payout CloudCoins       ");
            Console.Out.WriteLine("      This Software is provided as is with all faults, defects    ");
            Console.Out.WriteLine("          and errors, and without warranty of any kind.           ");
            Console.Out.WriteLine("                Free from the CloudCoin Consortium.               ");
            Console.Out.WriteLine("                                                                  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.Write("  Checking RAIDA");
            echoRaida();
            RAIDA_Status.showMs();
            //Check to see if suspect files need to be imported because they failed to finish last time. 
            String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                Console.ForegroundColor = ConsoleColor.White;
               
                //   import();//temp stop while testing, change this in production
                //   grade();

            } //end if there are files in the suspect folder that need to be imported

        } // End print welcome
        public static void help()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("Customer Service:");
            Console.Out.WriteLine("Open 9am to 11pm California Time(PST).");
            Console.Out.WriteLine("1 (530) 500 - 2646");
            Console.Out.WriteLine("CloudCoin.HelpDesk@gmail.com(unsecure)");
            Console.Out.WriteLine("CloudCoin.HelpDesk@Protonmail.com(secure if you get a free encrypted email account at ProtonMail.com)");

        }//End Help

        public static bool echoRaida()
        {
            RAIDA_Status.resetEcho();
            RAIDA raida1 = new RAIDA();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.Write(strPad + countries[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.Write(strPad + countries[i]);
                    totalReady++;
                }
                if (i == 4 || i == 9 || i == 14 || i == 19) { Console.WriteLine(); }
            }//end for
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.Write("  RAIDA Health: " + totalReady + " / 25: ");//"RAIDA Health: " + totalReady );

            //Check if enough are good 
            if (totalReady < 16)//
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  Not enough RAIDA servers can be contacted to import new coins.");// );
                Console.Out.WriteLine("  Is your device connected to the Internet?");// );
                Console.Out.WriteLine("  Is a router blocking your connection?");// );
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("The RAIDA is ready for counterfeit detection.");// );
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }//end if enough RAIDA
        }//End echo

        public static void showCoins()
        {
            Console.Out.WriteLine("");
            // This is for consol apps.
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //Output  " 12.3"
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("    Total Coins in Bank:    " + string.Format("{0,8:N0}", (bankTotals[0] + frackedTotals[0])) + "                                ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("                 1s         5s         25s       100s       250s    ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Perfect:   " + string.Format("{0,7}", bankTotals[1]) + "    " + string.Format("{0,7}", bankTotals[2]) + "    " + string.Format("{0,7}", bankTotals[3]) + "    " + string.Format("{0,7}", bankTotals[4]) + "    " + string.Format("{0,7}", bankTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Fracked:   " + string.Format("{0,7}", frackedTotals[1]) + "    " + string.Format("{0,7}", frackedTotals[2]) + "    " + string.Format("{0,7}", frackedTotals[3]) + "    " + string.Format("{0,7}", frackedTotals[4]) + "    " + string.Format("{0,7}", frackedTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

        }// end show

        public static void showFolders()
        {
            Console.Out.WriteLine(" Root:        " + rootFolder);
            Console.Out.WriteLine(" Import:      " + fileUtils.importFolder);
            Console.Out.WriteLine(" Imported:    " + fileUtils.importedFolder);
            Console.Out.WriteLine(" Suspect:     " + fileUtils.suspectFolder);
            Console.Out.WriteLine(" Trash:       " + fileUtils.trashFolder);
            Console.Out.WriteLine(" Bank:        " + fileUtils.bankFolder);
            Console.Out.WriteLine(" Fracked:     " + fileUtils.frackedFolder);
            Console.Out.WriteLine(" Templates:   " + fileUtils.templateFolder);
            Console.Out.WriteLine(" Directory:   " + fileUtils.directoryFolder);
            Console.Out.WriteLine(" Counterfeits:" + fileUtils.counterfeitFolder);
            Console.Out.WriteLine(" Export:      " + fileUtils.exportFolder);
            Console.Out.WriteLine(" Lost:        " + fileUtils.lostFolder);
        } // end show folders

        public static void import()
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
                Console.Out.WriteLine("You do not have enought RAIDA to perform an import operation.");
                Console.Out.WriteLine("Check to make sure your internet is working.");
                Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                Console.ForegroundColor = ConsoleColor.White;
                multi_detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(importFolder);
            Console.ForegroundColor = ConsoleColor.White;

            Importer importer = new Importer(fileUtils);
            if (!importer.importAll())//Moves all CloudCoins from the Import folder into the Suspect folder. 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("  No coins in import folder.");// "No coins in import folder.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                DateTime before = DateTime.Now;
                DateTime after;
                TimeSpan ts = new TimeSpan();
                //Console.Out.WriteLine("  IMPORT DONE> NOW DETECTING MULTI. Do you want to start detecting?");// "No coins in import folder.");
                // Console.In.ReadLine();
                multi_detect();
                // Console.Out.WriteLine("  DETCATION DONE> NOW GRADING. Do you want to start Grading?");// "No coins in import folder.");
                // Console.In.ReadLine();
                after = DateTime.Now;
                ts = after.Subtract(before);//end the timer
                
                grade();
                // Console.Out.WriteLine("  GRADING DONE NOW SHOWING. Do you wnat to show");// "No coins in import folder.");
                // Console.In.ReadLine();
                Console.Out.WriteLine("Time in ms to multi detect pown " + ts.TotalMilliseconds);
                RAIDA_Status.showMultiMs();
                showCoins();

            }//end if coins to import
        }   // end import


        public static void detect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils);
            int[] detectionResults = detector.detectAll(14000, 10000);
            Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
                                                                                         // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
        }//end detect

        public static void multi_detect() {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            MultiDetect multi_detector = new MultiDetect(fileUtils);

            //Calculate timeout
            int detectTime = 20000;
            if (RAIDA_Status.getLowest21() > detectTime)
            {
                detectTime = RAIDA_Status.getLowest21() + 200;
            }//Slow connection

            multi_detector.detectMulti(detectTime);

        }//end multi detect

        public static void grade()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Grading Authenticated Coins");// "Detecting Authentication of Suspect Coins");
            Grader grader = new Grader(fileUtils);
            int[] detectionResults = grader.gradeAll(5000, 2000);
            Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[1]);//"Total imported to fracked: "                                                                       // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[2]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            Console.Out.WriteLine("  Total moved to Lost folder: " + detectionResults[4]);//"Total Kept in suspect folder: " 

        }//end detect

        public static void dump()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Export for sales will export many stack files with only one note in them.");
            Console.Out.WriteLine("  Each file will recieve a random tag.");
            Console.Out.WriteLine("  This function helps you make CloudCoins for automated sales points.");
            Console.Out.WriteLine("  Continue to dump? 1.Yes or 2. Quit?");

            int okToDump = reader.readInt(1, 2);
            if (okToDump == 1)
            {
                Dumper dumper = new Dumper(fileUtils);
                Console.Out.WriteLine("");
                Banker bank = new Banker(fileUtils);
                int[] bankTotals = bank.countCoins(bankFolder);
                int[] frackedTotals = bank.countCoins(frackedFolder);
                Console.Out.WriteLine("  Your Bank Inventory:");
                int grandTotal = (bankTotals[0] + frackedTotals[0]);
                showCoins();
                // state how many 1, 5, 25, 100 and 250
                int exp_1 = 0;
                int exp_5 = 0;
                int exp_25 = 0;
                int exp_100 = 0;
                int exp_250 = 0;

                // 1 jpg 2 stack
                if ((bankTotals[1] + frackedTotals[1]) > 0)
                {
                    Console.Out.WriteLine("  How many 1s do you want to dump?");
                    exp_1 = reader.readInt(0, (bankTotals[1] + frackedTotals[1]));
                }

                // if 1s not zero 
                if ((bankTotals[2] + frackedTotals[2]) > 0)
                {
                    Console.Out.WriteLine("  How many 5s do you want to dump?");
                    exp_5 = reader.readInt(0, (bankTotals[2] + frackedTotals[2]));
                }

                // if 1s not zero 
                if ((bankTotals[3] + frackedTotals[3] > 0))
                {
                    Console.Out.WriteLine("  How many 25s do you want to dump?");
                    exp_25 = reader.readInt(0, (bankTotals[3] + frackedTotals[3]));
                }

                // if 1s not zero 
                if ((bankTotals[4] + frackedTotals[4]) > 0)
                {
                    Console.Out.WriteLine("  How many 100s do you want to dump?");
                    exp_100 = reader.readInt(0, (bankTotals[4] + frackedTotals[4]));
                }

                // if 1s not zero 
                if ((bankTotals[5] + frackedTotals[5]) > 0)
                {
                    Console.Out.WriteLine("  How many 250s do you want to dump?");
                    exp_250 = reader.readInt(0, (bankTotals[5] + frackedTotals[5]));
                }

                dumper.dumpSome(exp_1, exp_5, exp_25, exp_100, exp_250);



                Console.Out.WriteLine("  Export complete. Check your export folder");
                // And the bank and the fractured for total
                showCoins();
            }

        }//end dump

        public static void export()
        {
            Console.Out.WriteLine("");
            Banker bank = new Banker(fileUtils);
            int[] bankTotals = bank.countCoins(bankFolder);
            int[] frackedTotals = bank.countCoins(frackedFolder);
            Console.Out.WriteLine("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0]);
            showCoins();
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = 0;
            int exp_5 = 0;
            int exp_25 = 0;
            int exp_100 = 0;
            int exp_250 = 0;
            //Warn if too many coins
            Console.WriteLine(bankTotals[1] + frackedTotals[1] + bankTotals[2] + frackedTotals[2] + bankTotals[3] + frackedTotals[3] + bankTotals[4] + frackedTotals[4] + bankTotals[5] + frackedTotals[5]);
            if (((bankTotals[1] + frackedTotals[1]) + (bankTotals[2] + frackedTotals[2]) + (bankTotals[3] + frackedTotals[3]) + (bankTotals[4] + frackedTotals[4]) + (bankTotals[5] + frackedTotals[5])) > 1000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Warning: You have more than 1000 Notes in your bank. Stack files should not have more than 1000 Notes in them.");
                Console.Out.WriteLine("Do not export stack files with more than 1000 notes. .");
                Console.ForegroundColor = ConsoleColor.White;
            }//end if they have more than 1000 coins

            Console.Out.WriteLine("  Do you want to export your CloudCoin to (1)jpgs or (2) stack (JSON) file?");
            int file_type = reader.readInt(1, 2);
            // 1 jpg 2 stack
            if ((bankTotals[1] + frackedTotals[1]) > 0)
            {
                Console.Out.WriteLine("  How many 1s do you want to export?");
                exp_1 = reader.readInt(0, (bankTotals[1] + frackedTotals[1]));
            }

            // if 1s not zero 
            if ((bankTotals[2] + frackedTotals[2]) > 0)
            {
                Console.Out.WriteLine("  How many 5s do you want to export?");
                exp_5 = reader.readInt(0, (bankTotals[2] + frackedTotals[2]));
            }

            // if 1s not zero 
            if ((bankTotals[3] + frackedTotals[3] > 0))
            {
                Console.Out.WriteLine("  How many 25s do you want to export?");
                exp_25 = reader.readInt(0, (bankTotals[3] + frackedTotals[3]));
            }

            // if 1s not zero 
            if ((bankTotals[4] + frackedTotals[4]) > 0)
            {
                Console.Out.WriteLine("  How many 100s do you want to export?");
                exp_100 = reader.readInt(0, (bankTotals[4] + frackedTotals[4]));
            }

            // if 1s not zero 
            if ((bankTotals[5] + frackedTotals[5]) > 0)
            {
                Console.Out.WriteLine("  How many 250s do you want to export?");
                exp_250 = reader.readInt(0, (bankTotals[5] + frackedTotals[5]));
            }

            // if 1s not zero 
            // move to export
            Exporter exporter = new Exporter(fileUtils);
            if (file_type == 1)
            {
                Console.Out.WriteLine("  Tag your jpegs with 'random' to give them a random number.");
            }
            Console.Out.WriteLine("  What tag will you add to the file name?");
            String tag = reader.readString();
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
        }// end export One

        public static void fix(int millisecondsToFixOne)
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
            fixer.fixAll(millisecondsToFixOne);
            stopwatch.Stop();
            Console.Out.WriteLine("  Fix Time: " + stopwatch.Elapsed + " ms");
            showCoins();
            Console.Out.WriteLine("  If your coins are not completely fixed, you may 'fix fracked' again.");
        }//end fix

        public static void toMind()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Not Yet Implemented");
        }//end to mind                           


        public static void fromMind()
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Not Yet Implemented");
        }//end from mind


        public static async System.Threading.Tasks.Task testDAAsync(int raida_ID, int timeout)
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Testing DA");
            int[] nns = { 1, 1, 1, 1 };
            int[] sns = { 1271, 1272, 1273, 1274 };
            int[] dens = { 1, 1, 1, 1 };
            string[] ans = new string[4];
            string[] pans = new string[4];

            switch (raida_ID)
            {
                case 0:
                    ans[0] = "C53F4E879B08D422912677CD3DF8476F";
                    pans[0] = "C53F4E879B08D422912677CD3DF8476F";
                    ans[1] = "7DDC8CFD5B72E2E8CE0E666CD25479A7";
                    pans[1] = "7DDC8CFD5B72E2E8CE0E666CD25479A7";
                    ans[2] = "28C7A5394CEAB2261254CDA0D48FD000";
                    pans[2] = "28C7A5394CEAB2261254CDA0D48FD000";
                    ans[3] = "B083845E37B0BC0B129A5855DC82BDE9";
                    pans[3] = "B083845E37B0BC0B129A5855DC82BDE9";

                    break;
                case 1:
                    ans[0] = "35F82AC8B896D53FA0763380DAF897DC";
                    pans[0] = "35F82AC8B896D53FA0763380DAF897DC";
                    ans[1] = "63CC1AA105E849CEDE9F0233ECB76C09";
                    pans[1] = "63CC1AA105E849CEDE9F0233ECB76C09";
                    ans[2] = "16B3F35C44E5DEF7C8C32A676BDF2661";
                    pans[2] = "16B3F35C44E5DEF7C8C32A676BDF2661";
                    ans[3] = "CA16D3BB5A1865C8B5F4C43F3E46F78A";
                    pans[3] = "CA16D3BB5A1865C8B5F4C43F3E46F78A";

                    break;
                case 2:
                    ans[0] = "35F82AC8B896D53FA0763380DAF897DC";
                    pans[0] = "35F82AC8B896D53FA0763380DAF897DC";
                    ans[1] = "63CC1AA105E849CEDE9F0233ECB76C09";
                    pans[1] = "63CC1AA105E849CEDE9F0233ECB76C09";
                    ans[2] = "16B3F35C44E5DEF7C8C32A676BDF2661";
                    pans[2] = "16B3F35C44E5DEF7C8C32A676BDF2661";
                    ans[3] = "CA16D3BB5A1865C8B5F4C43F3E46F78A";
                    pans[3] = "CA16D3BB5A1865C8B5F4C43F3E46F78A";
                    break;
                case 3:
                    ans[0] = "E886BDA1E1C1BEBDF87BA47763ED5510";
                    pans[0] = "E886BDA1E1C1BEBDF87BA47763ED5510";
                    ans[1] = "833AC9EFA7D90239445D4350A46A787B";
                    pans[1] = "833AC9EFA7D90239445D4350A46A787B";
                    ans[2] = "CB8D89BD6831D57B451B3D0FF525EB27";
                    pans[2] = "CB8D89BD6831D57B451B3D0FF525EB27";
                    ans[3] = "DC362133C452A36E5428247411566CAB";
                    pans[3] = "DC362133C452A36E5428247411566CAB";
                    break;
                case 4:
                    ans[0] = "C4532860A07A8ECA1FD6C137626335A8";
                    pans[0] = "C4532860A07A8ECA1FD6C137626335A8";
                    ans[1] = "4B10351F1550CAE7EC30127BE1C91441";
                    pans[1] = "4B10351F1550CAE7EC30127BE1C91441";
                    ans[2] = "35822E7BC433A359EB76F2734B603570";
                    pans[2] = "35822E7BC433A359EB76F2734B603570";
                    ans[3] = "06C300EE7766502BA7F076B0739DC77C";
                    pans[3] = "06C300EE7766502BA7F076B0739DC77C";
                    break;
                case 5:
                    ans[0] = "915010AF981E95A765074144E9864EFF";
                    pans[0] = "915010AF981E95A765074144E9864EFF";
                    ans[1] = "993D7E32A32E8DB6D275FC3DF6180686";
                    pans[1] = "993D7E32A32E8DB6D275FC3DF6180686";
                    ans[2] = "7E8D0F7B26CE304D8AF921BE78A46A95";
                    pans[2] = "7E8D0F7B26CE304D8AF921BE78A46A95";
                    ans[3] = "6F48251B0CB9DEFD63AD4977513B640C";
                    pans[3] = "6F48251B0CB9DEFD63AD4977513B640C";
                    break;
                case 6:
                    ans[0] = "8FD38AC5B797384132AAD47940E1F1D1";
                    pans[0] = "8FD38AC5B797384132AAD47940E1F1D1";
                    ans[1] = "C0576F230EF2EA32FE2902B71224AEBC";
                    pans[1] = "C0576F230EF2EA32FE2902B71224AEBC";
                    ans[2] = "3396D21DFA4B28CD432A170D787FA123";
                    pans[2] = "3396D21DFA4B28CD432A170D787FA123";
                    ans[3] = "9223B7E61EBAEFC02AF42C0667C469E8";
                    pans[3] = "9223B7E61EBAEFC02AF42C0667C469E8";
                    break;
                case 7:
                    ans[0] = "B6C8F7E66E01C8A5534986BC79BF4975";
                    pans[0] = "B6C8F7E66E01C8A5534986BC79BF4975";
                    ans[1] = "DBE169AA055B14E68ED9B2E6325BCBC6";
                    pans[1] = "DBE169AA055B14E68ED9B2E6325BCBC6";
                    ans[2] = "F28B929395C2A77A271D7F4AAF1A1D0B";
                    pans[2] = "F28B929395C2A77A271D7F4AAF1A1D0B";
                    ans[3] = "42157ADA5307C4ED6E27CC7CEEA57DCC";
                    pans[3] = "42157ADA5307C4ED6E27CC7CEEA57DCC";
                    break;
                case 8:
                    ans[0] = "8FD38AC5B797384132AAD47940E1F1D1";
                    pans[0] = "8FD38AC5B797384132AAD47940E1F1D1";
                    ans[1] = "C0576F230EF2EA32FE2902B71224AEBC";
                    pans[1] = "C0576F230EF2EA32FE2902B71224AEBC";
                    ans[2] = "3396D21DFA4B28CD432A170D787FA123";
                    pans[2] = "3396D21DFA4B28CD432A170D787FA123";
                    ans[3] = "9223B7E61EBAEFC02AF42C0667C469E8";
                    pans[3] = "9223B7E61EBAEFC02AF42C0667C469E8";
                    break;
                case 9:
                    ans[0] = "0819EFFABA514BFA45FDE863AEB224D1";
                    pans[0] = "0819EFFABA514BFA45FDE863AEB224D1";
                    ans[1] = "A167201FE0949E8CC47FC8DE1EE4906C";
                    pans[1] = "A167201FE0949E8CC47FC8DE1EE4906C";
                    ans[2] = "AA4643A13B00017504FB3AF3D9CD294D";
                    pans[2] = "AA4643A13B00017504FB3AF3D9CD294D";
                    ans[3] = "95EEA1D40EFA05D1BD49CA99FE41085A";
                    pans[3] = "95EEA1D40EFA05D1BD49CA99FE41085A";
                    break;
                case 10:
                    ans[0] = "B3C8B3E8A6461A1821DA09D4B0BF6526";
                    pans[0] = "B3C8B3E8A6461A1821DA09D4B0BF6526";
                    ans[1] = "2CF7143F29BFBFB5AF8A95F505C1D5D9";
                    pans[1] = "2CF7143F29BFBFB5AF8A95F505C1D5D9";
                    ans[2] = "B094A5F2769B7F505F7F9EBEFA4E5888";
                    pans[2] = "B094A5F2769B7F505F7F9EBEFA4E5888";
                    ans[3] = "562AE114211F5502B182A31090587EE4";
                    pans[3] = "562AE114211F5502B182A31090587EE4";
                    break;
                case 11:
                    ans[0] = "B3C8B3E8A6461A1821DA09D4B0BF6526";
                    pans[0] = "B3C8B3E8A6461A1821DA09D4B0BF6526";
                    ans[1] = "2CF7143F29BFBFB5AF8A95F505C1D5D9";
                    pans[1] = "2CF7143F29BFBFB5AF8A95F505C1D5D9";
                    ans[2] = "B094A5F2769B7F505F7F9EBEFA4E5888";
                    pans[2] = "B094A5F2769B7F505F7F9EBEFA4E5888";
                    ans[3] = "562AE114211F5502B182A31090587EE4";
                    pans[3] = "562AE114211F5502B182A31090587EE4";
                    break;
                case 12:
                    ans[0] = "B3C8B3E8A6461A1821DA09D4B0BF6526";
                    pans[0] = "B3C8B3E8A6461A1821DA09D4B0BF6526";
                    ans[1] = "2CF7143F29BFBFB5AF8A95F505C1D5D9";
                    pans[1] = "2CF7143F29BFBFB5AF8A95F505C1D5D9";
                    ans[2] = "B094A5F2769B7F505F7F9EBEFA4E5888";
                    pans[2] = "B094A5F2769B7F505F7F9EBEFA4E5888";
                    ans[3] = "562AE114211F5502B182A31090587EE4";
                    pans[3] = "562AE114211F5502B182A31090587EE4";
                    break;
                case 13:
                    ans[0] = "0963B4C8BCE98A7D6ECE78A11FCD33FE";
                    pans[0] = "0963B4C8BCE98A7D6ECE78A11FCD33FE";
                    ans[1] = "E342C6443C298A7965ABCF72BDBBE0EC";
                    pans[1] = "E342C6443C298A7965ABCF72BDBBE0EC";
                    ans[2] = "99BE8720FDACF729763DE165A591B3FF";
                    pans[2] = "99BE8720FDACF729763DE165A591B3FF";
                    ans[3] = "BF6FC38FDAECADBD2280B712812865B7";
                    pans[3] = "BF6FC38FDAECADBD2280B712812865B7";
                    break;
                case 14:
                    ans[0] = "F02960F18488C9C24BE189F8592F8457";
                    pans[0] = "F02960F18488C9C24BE189F8592F8457";
                    ans[1] = "E0A90176807900F77D21AD2715B7124E";
                    pans[1] = "E0A90176807900F77D21AD2715B7124E";
                    ans[2] = "49E1909AE41D3F67EF49E0378830E872";
                    pans[2] = "49E1909AE41D3F67EF49E0378830E872";
                    ans[3] = "287FF93D7DD40A052D5B61135B521116";
                    pans[3] = "287FF93D7DD40A052D5B61135B521116";
                    break;
                case 15:
                    ans[0] = "73C366C3239FFA152D8D19E583EF0893";
                    pans[0] = "73C366C3239FFA152D8D19E583EF0893";
                    ans[1] = "CFE150089D77DA6ABCA4C9AEE0A244F8";
                    pans[1] = "CFE150089D77DA6ABCA4C9AEE0A244F8";
                    ans[2] = "647F14F97609161179293F8F84F6714D";
                    pans[2] = "647F14F97609161179293F8F84F6714D";
                    ans[3] = "5940EE55A70FAB9419B4EB9F33BF3F14";
                    pans[3] = "5940EE55A70FAB9419B4EB9F33BF3F14";
                    break;
                case 16:
                    ans[0] = "D07E5B0E246360FE44AA237C4E4A54FE";
                    pans[0] = "D07E5B0E246360FE44AA237C4E4A54FE";
                    ans[1] = "0279C1D55E6686E16FB40EB5EAD87B20";
                    pans[1] = "0279C1D55E6686E16FB40EB5EAD87B20";
                    ans[2] = "C32FE5BEC820825D5DAB40522D267045";
                    pans[2] = "C32FE5BEC820825D5DAB40522D267045";
                    ans[3] = "7DB99DAA483C9671573F1E3D7D2DF69F";
                    pans[3] = "7DB99DAA483C9671573F1E3D7D2DF69F";
                    break;
                case 17:
                    ans[0] = "BD03313C562C1B73BDAF82437A111F97";
                    pans[0] = "BD03313C562C1B73BDAF82437A111F97";
                    ans[1] = "A6A03CD6EEF9738C68167E2925809FE4";
                    pans[1] = "A6A03CD6EEF9738C68167E2925809FE4";
                    ans[2] = "F9451D6A0B19A218F7DCB77858F2221A";
                    pans[2] = "F9451D6A0B19A218F7DCB77858F2221A";
                    ans[3] = "729F8062EED9CE21FC4F61BCC9D88300";
                    pans[3] = "729F8062EED9CE21FC4F61BCC9D88300";
                    break;
                case 18:
                    ans[0] = "5F1B46961FA1005539552CA471752F92";
                    pans[0] = "5F1B46961FA1005539552CA471752F92";
                    ans[1] = "2276AAE8B2A3B79017C3AD049162FF0E";
                    pans[1] = "2276AAE8B2A3B79017C3AD049162FF0E";
                    ans[2] = "4D37990863AD1A97641231DF10DFA742";
                    pans[2] = "4D37990863AD1A97641231DF10DFA742";
                    ans[3] = "FCEA6FDC0A6E9413D1C8D70E1C7D9678";
                    pans[3] = "FCEA6FDC0A6E9413D1C8D70E1C7D9678";
                    break;
                case 19:
                    ans[0] = "6D991C1D369EF7292E9D3758029A0653";
                    pans[0] = "6D991C1D369EF7292E9D3758029A0653";
                    ans[1] = "9995FBA9084B64F55C86D074E31AE758";
                    pans[1] = "9995FBA9084B64F55C86D074E31AE758";
                    ans[2] = "EBFA443FE5AAB9D3C90DFAEADC24CACE";
                    pans[2] = "EBFA443FE5AAB9D3C90DFAEADC24CACE";
                    ans[3] = "6A232583004E3B98E89432FDA60D997F";
                    pans[3] = "6A232583004E3B98E89432FDA60D997F";
                    break;
                case 20:
                    ans[0] = "78D4A3D97CC1BA658AE5694BC1E1A94D";
                    pans[0] = "78D4A3D97CC1BA658AE5694BC1E1A94D";
                    ans[1] = "F2AA9E64C3C82820C08084EFD533FAC3";
                    pans[1] = "F2AA9E64C3C82820C08084EFD533FAC3";
                    ans[2] = "9C1CFBB664A6B3FC584BF8FAD638E6F5";
                    pans[2] = "9C1CFBB664A6B3FC584BF8FAD638E6F5";
                    ans[3] = "0B34A2A196EB89125F658E0CC3EC89F8";
                    pans[3] = "0B34A2A196EB89125F658E0CC3EC89F8";
                    break;
                case 21:
                    ans[0] = "3058998196FB638886069AC0B1B57AA2";
                    pans[0] = "3058998196FB638886069AC0B1B57AA2";
                    ans[1] = "A38974311C7E45F362B2359CC34C7FC8";
                    pans[1] = "A38974311C7E45F362B2359CC34C7FC8";
                    ans[2] = "00B02D64F8D285188370362326F951B2";
                    pans[2] = "00B02D64F8D285188370362326F951B2";
                    ans[3] = "93797AB4B0A9C8DEC89241D6179210FD";
                    pans[3] = "93797AB4B0A9C8DEC89241D6179210FD";
                    break;
                case 22:
                    ans[0] = "684B70EB64CA400D94F0D89EA476BA5D";
                    pans[0] = "684B70EB64CA400D94F0D89EA476BA5D";
                    ans[1] = "F2AA68FE9B4B7EFDA6A247902677F8A3";
                    pans[1] = "F2AA68FE9B4B7EFDA6A247902677F8A3";
                    ans[2] = "AED8BAA5A8884ABA9874EFBA56ED3310";
                    pans[2] = "AED8BAA5A8884ABA9874EFBA56ED3310";
                    ans[3] = "98CA2ABF7DAC4F55B1500BB038C10E48";
                    pans[3] = "98CA2ABF7DAC4F55B1500BB038C10E48";
                    break;
                case 23:
                    ans[0] = "235FA176FBCB47484BFA6E37B9AF1FBA";
                    pans[0] = "235FA176FBCB47484BFA6E37B9AF1FBA";
                    ans[1] = "009CC67A549FEF716D7B7597556A0D2D";
                    pans[1] = "009CC67A549FEF716D7B7597556A0D2D";
                    ans[2] = "241487458F244A3013940153E2AF2307";
                    pans[2] = "241487458F244A3013940153E2AF2307";
                    ans[3] = "93360B9225A596C563D6C168A10BE64A";
                    pans[3] = "93360B9225A596C563D6C168A10BE64A";
                    break;
                case 24:
                    ans[0] = "0E844AC1D5C4461F9EEF978A1B85D8AE";
                    pans[0] = "0E844AC1D5C4461F9EEF978A1B85D8AE";
                    ans[1] = "ECA861B359054331A2CCEE96D0BCBB60";
                    pans[1] = "ECA861B359054331A2CCEE96D0BCBB60";
                    ans[2] = "B8A3096704A1494BBDA3D0E3011244E1";
                    pans[2] = "B8A3096704A1494BBDA3D0E3011244E1";
                    ans[3] = "B4BF49BAE5714408AA9897FE75487DC0";
                    pans[3] = "B4BF49BAE5714408AA9897FE75487DC0";
                    break;
            }//end switch


            DetectionAgent da = new DetectionAgent(raida_ID);
            Console.Out.WriteLine("  Requesting Response");
            Response[] x = await da.multiDetect(nns, sns, ans, pans, dens, timeout);

            if (x.Length == 0)
            {
                Console.Out.WriteLine("No Response.");
            }//end if not response
            for (int i = 0; i < x.Length; i++)
            {
                Console.Out.WriteLine();
                Console.Out.WriteLine(x[i].fullRequest);
                Console.Out.WriteLine(x[i].fullResponse);
                Console.Out.WriteLine(x[i].success);
                Console.Out.WriteLine(x[i].outcome);
                Console.Out.WriteLine(x[i].milliseconds);
                Console.Out.WriteLine();
            }//end for each response

        }//end Test 

    }//End class
}//end namespace
