using System;
using System.IO;
using System.Linq;

namespace Founders
{
    public class Detector
    {
        /*  INSTANCE VARIABLES */
        RAIDA raida;
        FileUtils fileUtils;
        int detectTime = 5000;


        /*  CONSTRUCTOR */
        public Detector(FileUtils fileUtils, int timeout)
        {
            this.raida = new RAIDA(timeout);
            this.fileUtils = fileUtils;
        }// end Detect constructor


        /*  PUBLIC METHODS */
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] detectAll()
        {
            // LOAD THE .suspect COINS ONE AT A TIME AND TEST THEM
            int[] results = new int[4]; // [0] Coins to bank, [1] Coins to fracked [2] Coins to Counterfeit
            String[] suspectFileNames = new DirectoryInfo(this.fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            int totalValueToBank = 0;
            int totalValueToCounterfeit = 0;
            int totalValueToFractured = 0;
            int totalValueToKeptInSuspect = 0;
            bool coinSupect = false;
            CloudCoin newCC;
            for (int j = 0; j < suspectFileNames.Length; j++)
            {
                try
                {
                    if (File.Exists(this.fileUtils.bankFolder + suspectFileNames[j]))
                    {//Coin has already been imported. Delete it from import folder move to trash.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("You tried to import a coin that has already been imported.");
                        File.Move(this.fileUtils.suspectFolder + suspectFileNames[j], this.fileUtils.trashFolder + suspectFileNames[j]);
                        Console.Out.WriteLine("Suspect CloudCoin was moved to Trash folder.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[j]);
                        CoinUtils cu = new CoinUtils(newCC);
                        Console.Out.WriteLine("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                        Console.Out.WriteLine("");

                        CoinUtils detectedCC = this.raida.detectCoin(cu, detectTime);
                        cu.calcExpirationDate();

                        if (j == 0)//If we are detecting the first coin, note if the RAIDA are working
                        {
                            for (int i = 0; i < 25; i++)// Checks any servers are down so we don't try to check them again. 
                            {
                                if (cu.getPastStatus(i) != "pass" && cu.getPastStatus(i) != "fail")
                                {
                                    raida.raidaIsDetecting[i] = false;//Server is not working correctly, don't try it agian
                                }
                            }
                        }//end if it is the first coin we are detecting

                        cu.consoleReport();

                        bool alreadyExists = false;//Does the file already been imported?
                        switch ( cu.getFolder().ToLower())
                        {
                            case "bank":
                                totalValueToBank++;
                                alreadyExists = this.fileUtils.writeTo(this.fileUtils.bankFolder, detectedCC.cc);
                                break;
                            case "fracked":
                                totalValueToFractured++;
                                alreadyExists = this.fileUtils.writeTo(this.fileUtils.frackedFolder, detectedCC.cc);
                                break;
                            case "counterfeit":
                                totalValueToCounterfeit++;
                                alreadyExists = this.fileUtils.writeTo(this.fileUtils.counterfeitFolder, detectedCC.cc);
                                break;
                            case "suspect":
                                totalValueToKeptInSuspect++;
                                coinSupect = true;//Coin will remain in suspect folder
                                break;
                        }//end switch



                        // end switch on the place the coin will go 
                        if (!coinSupect)//Leave coin in the suspect folder if RAIDA is down
                        {
                            File.Delete(this.fileUtils.suspectFolder + suspectFileNames[j]);//Take the coin out of the suspect folder
                        }
                        else
                        {
                            this.fileUtils.writeTo(this.fileUtils.suspectFolder, detectedCC.cc);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLine("Not enough RAIDA were contacted to determine if the coin is authentic.");
                            Console.Out.WriteLine("Try again later.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }//end if else

                    }//end if file exists
                }
                catch (FileNotFoundException ex)
                {
                    Console.Out.WriteLine(ex);
                }
                catch (IOException ioex)
                {
                    Console.Out.WriteLine(ioex);
                }// end try catch
            }// end for each coin to import
            results[0] = totalValueToBank;
            results[1] = totalValueToCounterfeit;
            results[2] = totalValueToFractured;
            results[3] = totalValueToKeptInSuspect;
            return results;
        }//Detect All

    }
}
