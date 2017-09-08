using Founders;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Founders
{
    class Program
    {
        public static KeyboardReader reader = new KeyboardReader();
        //  public static String rootFolder = System.getProperty("user.dir") + File.separator +"bank" + File.separator ;
        public static String rootFolder = AppContext.BaseDirectory + Path.DirectorySeparatorChar;

        public static String prompt = "> ";
        public static String[] commandsAvailable = new String[] { "echo raida", "show coins", "import", "export", "fix fracked", "show folders", "export stack files with one note each", "quit" };

        //{ "echo raida", "show coins", "import", "export", "fix fracked", "show folders", "export for sales", "quit" };
        public static int timeout = 10000; // Milliseconds to wait until the request is ended. 
        public static FileUtils fileUtils = FileUtils.GetInstance(rootFolder);

        public static Random myRandom = new Random();


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

        public static void handleCommand(string[] args)
        {
            string command = args[0];

            switch (command)
            {
                case "--help":
                    printHelp();
                    break;
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
                    fix();
                    break;
                case "dump":
                    dump();
                    break;
                default:
                    break;
            }
        }
        public static void printHelp()
        {
            Console.WriteLine(" echo        :           Echo RAIDA");
            Console.WriteLine(" import      :           Import Coins");
            Console.WriteLine(" export      :           Export Coins");
            Console.WriteLine(" showfolders :           Show Folders");
            Console.WriteLine(" fix         :           Fix Fracked Coins");


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
                        fix();
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
                        partialImport();
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
            Console.Out.WriteLine("                      Version: July.31.2017                       ");
            Console.Out.WriteLine("          Used to Authenticate, Store and Payout CloudCoins       ");
            Console.Out.WriteLine("      This Software is provided as is with all faults, defects    ");
            Console.Out.WriteLine("          and errors, and without warranty of any kind.           ");
            Console.Out.WriteLine("                Free from the CloudCoin Consortium.               ");
            Console.Out.WriteLine("                                                                  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.Write("  Checking RAIDA");
            echoRaida();
            //Check to see if suspect files need to be imported because they failed to finish last time. 
            String[] suspectFileNames = new DirectoryInfo(fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                Console.ForegroundColor = ConsoleColor.White;
                detect();
            } //end if there are files in the suspect folder that need to be imported

        } // End print welcome

        public static bool echoRaida()
        {
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
                int padding = longestCountryName - RAIDA.countries[i].Length;
                string strPad = "";
                for (int j = 0; j < padding; j++)
                {
                    strPad += " ";
                }//end for padding
                 // Console.Out.Write(RAIDA_Status.failsEcho[i]);
                if (RAIDA_Status.failsEcho[i])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.Write(strPad + RAIDA.countries[i]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Out.Write(strPad + RAIDA.countries[i]);
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
            int[] bankTotals = bank.countCoins(fileUtils.bankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);
            // int[] counterfeitTotals = bank.countCoins( counterfeitFolder );

            //Output  " 12.3"
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("    Total Coins in Bank:    " + string.Format("{0,8:N0}", (bankTotals[0] + frackedTotals[0] + partialTotals[0])) + "                                ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("                 1s         5s         25s       100s       250s    ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Perfect:   " + string.Format("{0,7}", bankTotals[1]) + "    " + string.Format("{0,7}", bankTotals[2]) + "    " + string.Format("{0,7}", bankTotals[3]) + "    " + string.Format("{0,7}", bankTotals[4]) + "    " + string.Format("{0,7}", bankTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Partial:   " + string.Format("{0,7}", partialTotals[1]) + "    " + string.Format("{0,7}", partialTotals[2]) + "    " + string.Format("{0,7}", partialTotals[3]) + "    " + string.Format("{0,7}", partialTotals[4]) + "    " + string.Format("{0,7}", partialTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.Out.WriteLine("   Fracked:   " + string.Format("{0,7}", frackedTotals[1]) + "    " + string.Format("{0,7}", frackedTotals[2]) + "    " + string.Format("{0,7}", frackedTotals[3]) + "    " + string.Format("{0,7}", frackedTotals[4]) + "    " + string.Format("{0,7}", frackedTotals[5]) + "   ");
            Console.Out.WriteLine("                                                                    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

        }// end show

        public static void showFolders()
        {
            Console.Out.WriteLine(" Your Root folder is:" + "\n " + rootFolder);
            Console.Out.WriteLine(" Your Import folder is:" + "\n  " + fileUtils.importFolder);
            Console.Out.WriteLine(" Your Imported folder is:" + "\n  " + fileUtils.importedFolder);
            Console.Out.WriteLine(" Your Suspect folder is: " + "\n  " + fileUtils.suspectFolder);
            Console.Out.WriteLine(" Your Trash folder is:" + "\n  " + fileUtils.trashFolder);
            Console.Out.WriteLine(" Your Bank folder is:" + "\n  " + fileUtils.bankFolder);
            Console.Out.WriteLine(" Your Fracked folder is:" + "\n  " + fileUtils.frackedFolder);
            Console.Out.WriteLine(" Your Templates folder is:" + "\n  " + fileUtils.templateFolder);
            Console.Out.WriteLine(" Your Directory folder is:" + "\n  " + fileUtils.directoryFolder);
            Console.Out.WriteLine(" Your Counterfeits folder is:" + "\n  " + fileUtils.counterfeitFolder);
            Console.Out.WriteLine(" Your Export folder is:" + "\n  " + fileUtils.exportFolder);
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
            String[] suspectFileNames = new DirectoryInfo(fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                Console.ForegroundColor = ConsoleColor.White;
                detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(fileUtils.importFolder);
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
                detect();
            }//end if coins to import
        }   // end import

        public static void partialImport()
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
                Console.Out.WriteLine("You do not have enough RAIDA to perform an import operation.");
                Console.Out.WriteLine("Check to make sure your internet is working.");
                Console.Out.WriteLine("Make sure no routers at your work are blocking access to the RAIDA.");
                Console.Out.WriteLine("Try to Echo RAIDA and see if the status has changed.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            //CHECK TO SEE IF THERE ARE UN DETECTED COINS IN THE SUSPECT FOLDER
            String[] suspectFileNames = new DirectoryInfo(fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            if (suspectFileNames.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine("  Finishing importing coins from last time...");//
                Console.ForegroundColor = ConsoleColor.White;
                detect();
                Console.Out.WriteLine("  Now looking in import folder for new coins...");// "Now looking in import folder for new coins...");
            } //end if there are files in the suspect folder that need to be imported


            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Out.WriteLine("  Loading all CloudCoins in your import folder: ");// "Loading all CloudCoins in your import folder: " );
            Console.Out.WriteLine(fileUtils.importFolder);
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
                partialDetect();
            }//end if coins to import
        }   // end import


        public static void detect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout);
            int[] detectionResults = detector.detectAll();
            Console.Out.WriteLine("  Total imported to bank: " + detectionResults[0]);//"Total imported to bank: "
            Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
                                                                                         // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
        }//end detect

        public static void partialDetect()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.Out.WriteLine("");
            Console.Out.WriteLine("  Partially Detecting Authentication of Suspect Coins");// "Detecting Authentication of Suspect Coins");
            Detector detector = new Detector(fileUtils, timeout);
            int[] detectionResults = detector.partialDetectAll();
            Console.Out.WriteLine("  Total imported to partial: " + detectionResults[0]);//"Total imported to bank: "
            Console.Out.WriteLine("  Total imported to fracked: " + detectionResults[2]);//"Total imported to fracked: "
                                                                                         // And the bank and the fractured for total
            Console.Out.WriteLine("  Total Counterfeit: " + detectionResults[1]);//"Total Counterfeit: "
            Console.Out.WriteLine("  Total Kept in suspect folder: " + detectionResults[3]);//"Total Kept in suspect folder: " 
            showCoins();
            stopwatch.Stop();
            Console.Out.WriteLine(stopwatch.Elapsed + " ms");
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
                int[] bankTotals = bank.countCoins(fileUtils.bankFolder);
                int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
                int[] partialTotals = bank.countCoins(fileUtils.partialFolder);
                Console.Out.WriteLine("  Your Bank Inventory:");
                int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
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
                    exp_1 = reader.readInt(0, (bankTotals[1] + frackedTotals[1] + partialTotals[1]));
                }

                // if 1s not zero 
                if ((bankTotals[2] + frackedTotals[2]) > 0)
                {
                    Console.Out.WriteLine("  How many 5s do you want to dump?");
                    exp_5 = reader.readInt(0, (bankTotals[2] + frackedTotals[2] + partialTotals[2]));
                }

                // if 1s not zero 
                if ((bankTotals[3] + frackedTotals[3] > 0))
                {
                    Console.Out.WriteLine("  How many 25s do you want to dump?");
                    exp_25 = reader.readInt(0, (bankTotals[3] + frackedTotals[3] + partialTotals[3]));
                }

                // if 1s not zero 
                if ((bankTotals[4] + frackedTotals[4]) > 0)
                {
                    Console.Out.WriteLine("  How many 100s do you want to dump?");
                    exp_100 = reader.readInt(0, (bankTotals[4] + frackedTotals[4] + partialTotals[4]));
                }

                // if 1s not zero 
                if ((bankTotals[5] + frackedTotals[5]) > 0)
                {
                    Console.Out.WriteLine("  How many 250s do you want to dump?");
                    exp_250 = reader.readInt(0, (bankTotals[5] + frackedTotals[5] + partialTotals[5]));
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
            int[] bankTotals = bank.countCoins(fileUtils.bankFolder);
            int[] frackedTotals = bank.countCoins(fileUtils.frackedFolder);
            int[] partialTotals = bank.countCoins(fileUtils.partialFolder);
            Console.Out.WriteLine("  Your Bank Inventory:");
            int grandTotal = (bankTotals[0] + frackedTotals[0] + partialTotals[0]);
            showCoins();
            // state how many 1, 5, 25, 100 and 250
            int exp_1 = 0;
            int exp_5 = 0;
            int exp_25 = 0;
            int exp_100 = 0;
            int exp_250 = 0;
            //Warn if too many coins
            Console.WriteLine(bankTotals[1] + frackedTotals[1] + bankTotals[2] + frackedTotals[2] + bankTotals[3] + frackedTotals[3] + bankTotals[4] + frackedTotals[4] + bankTotals[5] + frackedTotals[5] + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]);
            if (((bankTotals[1] + frackedTotals[1]) + (bankTotals[2] + frackedTotals[2]) + (bankTotals[3] + frackedTotals[3]) + (bankTotals[4] + frackedTotals[4]) + (bankTotals[5] + frackedTotals[5]) + partialTotals[1] + partialTotals[2] + partialTotals[3] + partialTotals[4] + partialTotals[5]) > 1000)
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
                exp_1 = reader.readInt(0, (bankTotals[1] + frackedTotals[1] + partialTotals[1]));
            }

            // if 1s not zero 
            if ((bankTotals[2] + frackedTotals[2]) > 0)
            {
                Console.Out.WriteLine("  How many 5s do you want to export?");
                exp_5 = reader.readInt(0, (bankTotals[2] + frackedTotals[2] + partialTotals[2]));
            }

            // if 1s not zero 
            if ((bankTotals[3] + frackedTotals[3] > 0))
            {
                Console.Out.WriteLine("  How many 25s do you want to export?");
                exp_25 = reader.readInt(0, (bankTotals[3] + frackedTotals[3] + partialTotals[3]));
            }

            // if 1s not zero 
            if ((bankTotals[4] + frackedTotals[4]) > 0)
            {
                Console.Out.WriteLine("  How many 100s do you want to export?");
                exp_100 = reader.readInt(0, (bankTotals[4] + frackedTotals[4] + partialTotals[4]));
            }

            // if 1s not zero 
            if ((bankTotals[5] + frackedTotals[5]) > 0)
            {
                Console.Out.WriteLine("  How many 250s do you want to export?");
                exp_250 = reader.readInt(0, (bankTotals[5] + frackedTotals[5] + partialTotals[5]));
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

        public static void fix()
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

        public static void testMind()
        {
            Console.Out.WriteLine("username: ");
            string user = reader.readString();
            Console.Out.WriteLine("password: ");
            string pass = reader.readString();
            Console.Out.WriteLine("email: ");
            string email = reader.readString();
            MindStorage mindStorage = new MindStorage();
            string[] pans = mindStorage.generateNewPan(user, pass, email);

            for (int i = 0; i < 25; i++)
                Console.Out.WriteLine(pans[i]);

        }

    }
}