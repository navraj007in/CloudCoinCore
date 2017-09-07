using System;
using System.IO;
using System.Linq;

namespace Founders
{
    public class Frack_Fixer
    {
        /* INSTANCE VARIABLES */
        private FileUtils fileUtils;
        private int totalValueToBank;
        private int totalValueToFractured;
        private int totalValueToCounterfeit;
        private RAIDA raida;
        

        /* CONSTRUCTORS */
        public Frack_Fixer(FileUtils fileUtils, int timeout)
        {
            
            this.fileUtils = fileUtils;
            raida = new RAIDA(timeout);
            totalValueToBank = 0;
            totalValueToCounterfeit = 0;
            totalValueToFractured = 0;
        }//constructor

        public string fixOneGuidCorner(int raida_ID, CloudCoin cc, int corner, int[] trustedTriad)
        {
            CoinUtils cu = new CoinUtils(cc);

            /*1. WILL THE BROKEN RAIDA FIX? check to see if it has problems echo, detect, or fix. */
            if (RAIDA_Status.failsFix[raida_ID] || RAIDA_Status.failsEcho[raida_ID] || RAIDA_Status.failsEcho[raida_ID])
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("");
                Console.Out.WriteLine("RAIDA Fails Echo or Fix. Try again when RAIDA online.");
                CoreLogger.Log("RAIDA Fails Echo or Fix. Try again when RAIDA online.");
                Console.Out.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
                return "RAIDA Fails Echo or Fix. Try again when RAIDA online.";
            }
            else
            {
                /*2. ARE ALL TRUSTED RAIDA IN THE CORNER READY TO HELP?*/
                //  Console.Out.WriteLine("Fails echo 0 " + RAIDA_Status.failsEcho[trustedTriad[0]]);
                //  Console.Out.WriteLine("Fails echo 1 " + RAIDA_Status.failsEcho[trustedTriad[1]]);
                //  Console.Out.WriteLine("Fails echo 2 " + RAIDA_Status.failsEcho[trustedTriad[2]]);
                //  Console.Out.WriteLine("Fails failsDetect 0 " + RAIDA_Status.failsDetect[trustedTriad[0]]);
                // Console.Out.WriteLine("Fails failsDetect 1 " + RAIDA_Status.failsDetect[trustedTriad[1]]);
                // Console.Out.WriteLine("Fails failsDetect 2 " + RAIDA_Status.failsDetect[trustedTriad[2]]);

                if (!RAIDA_Status.failsEcho[trustedTriad[0]] || !RAIDA_Status.failsDetect[trustedTriad[0]] || !RAIDA_Status.failsEcho[trustedTriad[1]] || !RAIDA_Status.failsDetect[trustedTriad[1]] || !RAIDA_Status.failsEcho[trustedTriad[2]] || !RAIDA_Status.failsDetect[trustedTriad[2]])
                {
                    /*3. GET TICKETS AND UPDATE RAIDA STATUS TICKETS*/
                    string[] ans = { cc.an[trustedTriad[0]], cc.an[trustedTriad[1]], cc.an[trustedTriad[2]] };
                    raida.get_Tickets(trustedTriad, ans, cc.nn, cc.sn, cu.getDenomination(), 3000);
                    /*4. ARE ALL TICKETS GOOD?*/
                    if (RAIDA_Status.hasTicket[trustedTriad[0]] && RAIDA_Status.hasTicket[trustedTriad[0]] && RAIDA_Status.hasTicket[trustedTriad[0]])
                    {
                        /*5.T YES, so REQUEST FIX*/
                        DetectionAgent da = new DetectionAgent(raida_ID, 5000);
                        Response fixResponse = da.fix(trustedTriad, RAIDA_Status.tickets[trustedTriad[0]], RAIDA_Status.tickets[trustedTriad[1]], RAIDA_Status.tickets[trustedTriad[2]], cc.an[raida_ID]).Result;
                        /*6. DID THE FIX WORK?*/
                        if (fixResponse.success)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Out.WriteLine("");
                            Console.Out.WriteLine("RAIDA" + raida_ID + " unfracked successfully.");
                            CoreLogger.Log("RAIDA" + raida_ID + " unfracked successfully.");
                            Console.Out.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.White;
                            return "RAIDA" + raida_ID + " unfracked successfully.";

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLine("");
                            Console.Out.WriteLine("RAIDA failed to accept tickets on corner " + corner);
                            CoreLogger.Log("RAIDA failed to accept tickets on corner " + corner);
                            Console.Out.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.White;
                            return "RAIDA failed to accept tickets on corner " + corner;
                        }//end if fix respons was success or fail
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("");
                        Console.Out.WriteLine("Trusted servers failed to provide tickets for corner " + corner);
                        CoreLogger.Log("Trusted servers failed to provide tickets for corner " + corner);
                        Console.Out.WriteLine("");
                        Console.ForegroundColor = ConsoleColor.White;

                        return "Trusted servers failed to provide tickets for corner " + corner;//no three good tickets
                    }//end if all good
                }//end if trused triad will echo and detect (Detect is used to get ticket)

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("");
                Console.Out.WriteLine("One or more of the trusted triad will not echo and detect.So not trying.");
                CoreLogger.Log("One or more of the trusted triad will not echo and detect.So not trying.");
                Console.Out.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
                return "One or more of the trusted triad will not echo and detect. So not trying.";
            }//end if RAIDA fails to fix. 

        }//end fix one



        /* PUBLIC METHODS */
        public int[] fixAll()
        {
            int[] results = new int[3];
            String[] frackedFileNames = new DirectoryInfo(this.fileUtils.frackedFolder).GetFiles().Select(o => o.Name).ToArray();
            CloudCoin frackedCC;
            //CoinUtils cu = new CoinUtils(frackedCC);
            if (frackedFileNames.Length < 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Out.WriteLine( "You have no fracked coins.");
                CoreLogger.Log("You have no fracked coins.");
                Console.ForegroundColor = ConsoleColor.White;
            }//no coins to unfrack


            for (int i = 0; i < frackedFileNames.Length; i++)
            {
                Console.WriteLine("UnFracking coin " + (i + 1) + " of " + frackedFileNames.Length);
                CoreLogger.Log("UnFracking coin " + (i + 1) + " of " + frackedFileNames.Length);
                try
                {
                    frackedCC = fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.frackedFolder + frackedFileNames[i]);
                    CoinUtils cu = new CoinUtils(frackedCC);
                    String value = frackedCC.pown;
                    //  Console.WriteLine("Fracked Coin: ");
                    cu.consoleReport();

                    CoinUtils fixedCC = fixCoin(frackedCC); // Will attempt to unfrack the coin. 

                    cu.consoleReport();
                    switch (fixedCC.getFolder().ToLower() )
                    {
                        case "bank":
                            this.totalValueToBank++;
                            this.fileUtils.overWrite(this.fileUtils.bankFolder, fixedCC.cc);
                            this.deleteCoin(this.fileUtils.frackedFolder + frackedFileNames[i]);
                            Console.WriteLine("CloudCoin was moved to Bank.");
                            CoreLogger.Log("CloudCoin was moved to Bank.");
                            break;
                        case "counterfeit":
                            this.totalValueToCounterfeit++;
                            this.fileUtils.overWrite(this.fileUtils.counterfeitFolder, fixedCC.cc);
                            this.deleteCoin(this.fileUtils.frackedFolder + frackedFileNames[i]);
                            Console.WriteLine("CloudCoin was moved to Trash.");
                            CoreLogger.Log("CloudCoin was moved to Trash.");
                            break;
                        default://Move back to fracked folder
                            this.totalValueToFractured++;
                            this.deleteCoin(this.fileUtils.frackedFolder + frackedFileNames[i]);
                            this.fileUtils.overWrite(this.fileUtils.frackedFolder, fixedCC.cc);
                            Console.WriteLine("CloudCoin was moved back to Fraked folder.");
                            CoreLogger.Log("CloudCoin was moved back to Fraked folder.");
                            break;
                    }
                    // end switch on the place the coin will go 
                    Console.WriteLine("...................................");
                    Console.WriteLine("");
                }
                catch (FileNotFoundException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex);
                    CoreLogger.Log(ex.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                }
                catch (IOException ioex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ioex);
                    CoreLogger.Log(ioex.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                } // end try catch
            }// end for each file name that is fracked

            results[0] = this.totalValueToBank;
            results[1] = this.totalValueToCounterfeit; // System.out.println("Counterfeit and Moved to trash: "+totalValueToCounterfeit);
            results[2] = this.totalValueToFractured; // System.out.println("Fracked and Moved to Fracked: "+ totalValueToFractured);
            return results;
        }// end fix all

        // End select all file names in a folder
        public bool deleteCoin(String path)
        {
            bool deleted = false;

            // System.out.println("Deleteing Coin: "+path + this.fileName + extension);
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CoreLogger.Log(e.ToString());
            }
            return deleted;
        }//end delete coin


        public CoinUtils fixCoin(CloudCoin brokeCoin)
        {
            CoinUtils cu = new CoinUtils(brokeCoin);

            /*0. RESET TICKETS IN RAIDA STATUS TO EMPTY*/
            RAIDA_Status.resetTickets();
            /*0. RESET THE DETECTION to TRUE if it is a new COIN */
            RAIDA_Status.newCoin();

            cu.setAnsToPans();// Make sure we set the RAIDA to the cc ans and not new pans. 
            DateTime before = DateTime.Now;

            String fix_result = "";
            FixitHelper fixer;

            /*START*/
            /*1. PICK THE CORNER TO USE TO TRY TO FIX */
            int corner = 1;
            // For every guid, check to see if it is fractured
            for (int raida_ID = 0; raida_ID < 25; raida_ID++)
            {
                //  Console.WriteLine("Past Status for " + raida_ID + ", " + brokeCoin.pastStatus[raida_ID]);

                if ( cu.getPastStatus(raida_ID).ToLower() != "pass")//will try to fix everything that is not perfect pass.
                {

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Out.WriteLine("");
                    Console.WriteLine("Attempting to fix RAIDA " + raida_ID);
                    CoreLogger.Log("Attempting to fix RAIDA " + raida_ID);
                    Console.Out.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.White;

                    fixer = new FixitHelper( raida_ID, brokeCoin.an.ToArray() );

                    //trustedServerAns = new String[] { brokeCoin.ans[fixer.currentTriad[0]], brokeCoin.ans[fixer.currentTriad[1]], brokeCoin.ans[fixer.currentTriad[2]] };
                    corner = 1;
                    while (!fixer.finnished)
                    {
                        Console.WriteLine(" Using corner " + corner);
                        CoreLogger.Log(" Using corner " + corner);
                        fix_result = fixOneGuidCorner(raida_ID, brokeCoin, corner, fixer.currentTriad);
                        // Console.WriteLine(" fix_result: " + fix_result + " for corner " + corner);
                        if (fix_result.Contains("success"))
                        {
                            //Fixed. Do the fixed stuff
                            cu.setPastStatus("pass", raida_ID);
                            fixer.finnished = true;
                            corner = 1;
                        }
                        else
                        {
                            //Still broken, do the broken stuff. 
                            corner++;
                            fixer.setCornerToCheck(corner);
                        }
                    }//End whild fixer not finnished
                }//end if RAIDA past status is passed and does not need to be fixed
            }//end for each AN

            for (int raida_ID = 24; raida_ID > 0; raida_ID--)
            {
                //  Console.WriteLine("Past Status for " + raida_ID + ", " + brokeCoin.pastStatus[raida_ID]);

                if (cu.getPastStatus(raida_ID).ToLower() != "pass")//will try to fix everything that is not perfect pass.
                {

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Out.WriteLine("");
                    Console.WriteLine("Attempting to fix RAIDA " + raida_ID);
                    CoreLogger.Log("Attempting to fix RAIDA " + raida_ID);
                    Console.Out.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.White;

                    fixer = new FixitHelper(raida_ID, brokeCoin.an.ToArray());

                    //trustedServerAns = new String[] { brokeCoin.ans[fixer.currentTriad[0]], brokeCoin.ans[fixer.currentTriad[1]], brokeCoin.ans[fixer.currentTriad[2]] };
                    corner = 1;
                    while (!fixer.finnished)
                    {
                        Console.WriteLine(" Using corner " + corner);
                        CoreLogger.Log(" Using corner " + corner);
                        fix_result = fixOneGuidCorner(raida_ID, brokeCoin, corner, fixer.currentTriad);
                        // Console.WriteLine(" fix_result: " + fix_result + " for corner " + corner);
                        if (fix_result.Contains("success"))
                        {
                            //Fixed. Do the fixed stuff
                            cu.setPastStatus("pass", raida_ID);
                            fixer.finnished = true;
                            corner = 1;
                        }
                        else
                        {
                            //Still broken, do the broken stuff. 
                            corner++;
                            fixer.setCornerToCheck(corner);
                        }
                    }//End whild fixer not finnished
                }//end if RAIDA past status is passed and does not need to be fixed
            }//end for each AN
            DateTime after = DateTime.Now;
            TimeSpan ts = after.Subtract(before);
            Console.WriteLine("Time spent fixing RAIDA in milliseconds: " + ts.Milliseconds);
            CoreLogger.Log("Time spent fixing RAIDA in milliseconds: " + ts.Milliseconds);

            cu.calculateHP();//how many fails did it get
          //  cu.gradeCoin();// sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            
            cu.grade();
            cu.calcExpirationDate();
            return cu;
        }// end fix coin

    }//end class
}//end namespace
