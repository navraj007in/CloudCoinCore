using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Founders
{
    public class Grader
    {
        /*  INSTANCE VARIABLES */
        private FileUtils fileUtils;
        int[] results = new int[5]; // [0] Coins to bank, [1] Coins to fracked [2] Coins to Counterfeit [3] suspect [4] lost

        /*  CONSTRUCTOR */
        public Grader(FileUtils fileUtils)
        {
            this.fileUtils = fileUtils;
            results[0] = 0;//Coins in bank
            results[1] = 0;//Coins to fracked
            results[2] = 0;//Coins to Counterfeit
            results[3] = 0;//Coins kept in suspect
            results[4] = 0;//Coins lost in process (no response from server)
        }// end Detect constructor


        /*  PUBLIC METHODS */
        public int[] gradeAll(int msToFixDangerousFracked, int msToRedetectDangerous)
        {
  
            String[] detectedFileNames = new DirectoryInfo(this.fileUtils.detectedFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            int totalValueToBank = 0;
            int totalValueToCounterfeit = 0;
            int totalValueToFractured = 0;
            int totalValueToKeptInSuspect = 0;
            int totalValueToLost = 0;
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
                        File.Move(this.fileUtils.detectedFolder + detectedFileNames[j], this.fileUtils.trashFolder + detectedFileNames[j]);
                        Console.Out.WriteLine("Suspect CloudCoin was moved to Trash folder.");
                        CoreLogger.Log("Suspect CloudCoin was moved to Trash folder.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.detectedFolder + detectedFileNames[j]);
                        CoinUtils cu = new CoinUtils(newCC);

                        //CoinUtils detectedCC = cu;
                        //cu.sortToFolder();//Tells the Coin Utils to set what folder the coins should go to. 
                        cu.consoleReport();
                      

                        //Suspect, Counterfeit, Fracked, Bank, Trash, Detected, Lost, Dangerous 
                        switch (cu.getFolder().ToLower())
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

                            case "lost":
                                totalValueToLost++;
                                fileUtils.writeTo(this.fileUtils.lostFolder, cu.cc);
                                break;

                            case "suspect":
                                totalValueToKeptInSuspect++;
                                fileUtils.writeTo(this.fileUtils.suspectFolder, cu.cc);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Out.WriteLine("  Not enough RAIDA were contacted to determine if the coin is authentic.");
                                Console.Out.WriteLine("  Try again later.");
                                CoreLogger.Log("  Not enough RAIDA were contacted to determine if the coin is authentic. Try again later.");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;

                            case "dangerous":
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("   WARNING: Strings may be attached to this coins");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Out.WriteLine("  Now fixing fracked for " + (j + 1) + " of " + detectedFileNames.Length + " . SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                                CoreLogger.Log("  Now fixing fracked for " + (j + 1) + " of " + detectedFileNames.Length + " . SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());

                                Frack_Fixer ff = new Frack_Fixer(fileUtils, msToFixDangerousFracked);
                                RAIDA raida = new RAIDA();
                                Console.WriteLine("folder is " + cu.getFolder().ToLower());
                                while (cu.getFolder().ToLower() == "dangerous")
                                {// keep fracking fixing until all fixed or no more improvments possible. 
                                    Console.WriteLine("   calling fix Coin");
                                    cu = ff.fixCoin(cu.cc, msToFixDangerousFracked);
                                    Console.WriteLine("   sorting after fixing");
                                    cu.sortFoldersAfterFixingDangerous();
                                }//while folder still dangerous

                                for (int i = 0; i < 25; i++) { cu.pans[i] = cu.generatePan(); } // end for each pan
                                cu = raida.detectCoin(cu, msToRedetectDangerous);//Detect again to make sure it is powned
                                cu.consoleReport();
                                cu.sortToFolder();//Tells the Coin Utils to set what folder the coins should go to. 
                                switch (cu.getFolder().ToLower()) {
                                    case "bank":
                                        totalValueToBank++;
                                        fileUtils.writeTo(this.fileUtils.bankFolder, cu.cc);
                                        break;

                                    case "fracked":
                                        totalValueToFractured++;
                                        fileUtils.writeTo(this.fileUtils.frackedFolder, cu.cc);
                                        break;

                                    default:
                                        totalValueToCounterfeit++;
                                        fileUtils.writeTo(this.fileUtils.counterfeitFolder, cu.cc);
                                        break;

                                }//end switch 

                                break;
                        }//end switch

                        File.Delete(this.fileUtils.detectedFolder + detectedFileNames[j]);//Take the coin out of the detected folder

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
            results[1] = totalValueToFractured;
            results[2] = totalValueToCounterfeit;
            results[3] = totalValueToKeptInSuspect;
            results[4] = totalValueToLost;
            return results;
        }//Detect All

    }// end class
}// end namespace
