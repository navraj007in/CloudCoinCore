using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Founders
{
     class Grader
    {
        /*  INSTANCE VARIABLES */
        private FileUtils fileUtils;
        int[] results = new int[4]; // [0] Coins to bank, [1] Coins to fracked [2] Coins to Counterfeit

        /*  CONSTRUCTOR */
        public Grader(FileUtils fileUtils)
        {
            this.fileUtils = fileUtils;
            results[0] = 0;//Coins in bank
            results[1] = 0;//Coins to fracked
            results[2] = 0;//Coins to Counterfeit
            results[3] = 0;//Coins kept in suspect
        }// end Detect constructor


        /*  PUBLIC METHODS */
        public int[] gradeAll()
        {
  
            String[] detectedFileNames = new DirectoryInfo(this.fileUtils.detectedFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            int totalValueToBank = 0;
            int totalValueToCounterfeit = 0;
            int totalValueToFractured = 0;
            int totalValueToKeptInSuspect = 0;
            bool coinSuspect = false;
            CloudCoin newCC;


            for (int j = 0; j < detectedFileNames.Length; j++)//for every coins in the detected folder
            {
                try
                {
                    if (File.Exists(this.fileUtils.bankFolder + detectedFileNames[j]))
                    {//Coin has already been imported. Delete it from import folder move to trash.
                        //THIS SHOULD NOT HAPPEN - THE COIN SHOULD HAVE BEEN CHECKED DURING IMPORT BEFORE DETECTION TO SEE IF IT WAS IN THE BANK FOLDER
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("You tried to import a coin that has already been imported.");
                        CoreLogger.Log("You tried to import a coin that has already been imported.");
                        File.Move(this.fileUtils.suspectFolder + detectedFileNames[j], this.fileUtils.trashFolder + detectedFileNames[j]);
                        Console.Out.WriteLine("Suspect CloudCoin was moved to Trash folder.");
                        CoreLogger.Log("Suspect CloudCoin was moved to Trash folder.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.detectedFolder + detectedFileNames[j]);
                        CoinUtils cu = new CoinUtils(newCC);
                    
                        //CoinUtils detectedCC = cu;

                        cu.consoleReport();
                        cu.sortToFolder();//Tells the Coin Utils to set what folder the coins should go to. 

                        /*IS DANGEROUS? See if coin could be taken back by previous owner*/
                        if (cu.getFolder() == "dangerous") {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("   WARNING: Strings may be attached to this coins");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Out.WriteLine("  Now scanning coin " + (j + 1) + " of " + detectedFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                            CoreLogger.Log("  Now scanning coin " + (j + 1) + " of " + detectedFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());

                            Frack_Fixer ff = new Frack_Fixer(fileUtils, 10000);
                            RAIDA raida = new RAIDA(5000);

                            while ( cu.getFolder() == "dangerous") {// keep fracking fixing until all fixed or no more improvments possible. 
                                cu = ff.fixCoin(cu.cc);
                                cu.sortFoldersAfterFixingDangerous();
                            }//while folder still dangerous

                            for (int i = 0; i < 25; i++) { cu.pans[i] = cu.generatePan(); } // end for each pan
                            cu = raida.detectCoin(cu, 5000);//Detect again to make sure it is powned
                            cu.consoleReport();
                            cu.sortToFolder();//Tells the Coin Utils to set what folder the coins should go to. 
                            //Check if the coin
                        }//End if coin is dangerous, must be fixed before authentication


                        //Suspect, Counterfeit, Fracked, Bank, Trash, Detected, Lost, Dangerous
                        switch (cu.getFolder().ToLower() )
                        {
                            case "bank":
                                totalValueToBank++;
                                fileUtils.writeTo(this.fileUtils.bankFolder, cu.cc);
                                break;
                            case "fracked":
                                totalValueToFractured++;
                                fileUtils.writeTo(this.fileUtils.frackedFolder, cu.cc);
                                break;
                            case "counterfeit":
                                totalValueToCounterfeit++;
                                fileUtils.writeTo(this.fileUtils.counterfeitFolder, cu.cc);
                                break;
                            case "suspect":
                                totalValueToKeptInSuspect++;
                                coinSuspect = true;//Coin will remain in suspect folder
                                break;
                            case "dangerous":
                                totalValueToCounterfeit++;
                                fileUtils.writeTo(this.fileUtils.counterfeitFolder, cu.cc);
                                break;
                        }//end switch



                        // end switch on the place the coin will go 
                        if (!coinSuspect)//Leave coin in the suspect folder if RAIDA is down
                        {
                            File.Delete(this.fileUtils.suspectFolder + detectedFileNames[j]);//Take the coin out of the suspect folder
                        }
                        else
                        {
                            fileUtils.writeTo(this.fileUtils.suspectFolder, cu.cc);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLine("  Not enough RAIDA were contacted to determine if the coin is authentic.");
                            Console.Out.WriteLine("  Try again later.");
                            CoreLogger.Log("  Not enough RAIDA were contacted to determine if the coin is authentic. Try again later.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }//end if else

                    }//end if file exists
                }
                catch (FileNotFoundException ex)
                {
                    Console.Out.WriteLine(ex);
                    CoreLogger.Log(ex.ToString());
                }
                catch (IOException ioex)
                {
                    Console.Out.WriteLine(ioex);
                    CoreLogger.Log(ioex.ToString());
                }// end try catch
            }// end for each coin to import
            results[0] = totalValueToBank;
            results[1] = totalValueToCounterfeit;
            results[2] = totalValueToFractured;
            results[3] = totalValueToKeptInSuspect;
            return results;
        }//Detect All

    }// end class
}// end namespace
