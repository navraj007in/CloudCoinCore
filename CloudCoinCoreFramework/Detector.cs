using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Founders
{
    public class Detector
    {
        /*  INSTANCE VARIABLES */
         RAIDA raida;
         FileUtils fileUtils;



        /*  CONSTRUCTOR */
        public Detector(FileUtils fileUtils)
        {
            raida = new RAIDA();
            this.fileUtils = fileUtils;
        }// end Detect constructor


        /*  PUBLIC METHODS */
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int[]> detectAll(int fixTime, int detectTime )
        {
            // LOAD THE .suspect COINS ONE AT A TIME AND TEST THEM
            int[] results = new int[4]; // [0] Coins to bank, [1] Coins to fracked [2] Coins to Counterfeit
            String[] suspectFileNames = new DirectoryInfo(this.fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            int totalValueToBank = 0;
            int totalValueToCounterfeit = 0;
            int totalValueToFractured = 0;
            int totalValueToKeptInSuspect = 0;
            bool coinSuspect = false;
            CloudCoin newCC;
            for (int j = 0; j < suspectFileNames.Length; j++)
            {
                try
                {
                    if (File.Exists(this.fileUtils.bankFolder + suspectFileNames[j]))
                    {//Coin has already been imported. Delete it from import folder move to trash.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("You tried to import a coin that has already been imported.");
                        CoreLogger.Log("You tried to import a coin that has already been imported.");
                        File.Move(this.fileUtils.suspectFolder + suspectFileNames[j], this.fileUtils.trashFolder + suspectFileNames[j]);
                        Console.Out.WriteLine("Suspect CloudCoin was moved to Trash folder.");
                        CoreLogger.Log("Suspect CloudCoin was moved to Trash folder.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[j]);
                        CoinUtils cu = new CoinUtils(newCC);
                        Console.Out.WriteLine("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                        CoreLogger.Log("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                        Console.Out.WriteLine("");

                        CoinUtils detectedCC = await this.raida.detectCoin(cu, detectTime);
                        cu.calcExpirationDate();
                        int numOfFails = 0;
                        if (j == 0)//If we are detecting the first coin, note if the RAIDA are working
                        {
                            for (int i = 0; i < 25; i++)// Checks any servers are down so we don't try to check them again. 
                            {
                                if (cu.getPastStatus(i) != "pass" && cu.getPastStatus(i) != "fail")
                                {
                                    raida.raidaIsDetecting[i] = false;//Server is not working correctly, don't try it agian
                                }
                                else if (cu.getPastStatus(i) == "fail")
                                {
                                    numOfFails++;
                                }
                            }
                        }//end if it is the first coin we are detecting

                        cu.consoleReport();
                        if (numOfFails > 5)
                        {
                            //Check for threats.
                            if (containsThreat(cu.cc.pown))
                            {  //This coin may be trying to charge back
                                Frack_Fixer ff = new Frack_Fixer(fileUtils, 10000);
                                cu = ff.fixCoin(cu.cc, fixTime);
                                for(int i = 0; i < 25; i++) { cu.pans[i] = cu.generatePan(); } // end for each pan
                                cu = await this.raida.detectCoin(cu, detectTime);
                                //Check if the number of fails is now below 5. 
                                int failCount = cu.cc.pown.Split('f').Length - 1;
                                //if it is above 5, make it counterfeit. Powning is not working.
                                if ( failCount > 5 ) {
                                    cu.setFolder( "counterfeit" );
                                    cu.consoleReport();
                                }//end if over 5
                            }
                        }



                        bool alreadyExists = false;//Does the file already been imported?
                        switch (cu.getFolder().ToLower())
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
                                coinSuspect = true;//Coin will remain in suspect folder
                                break;
                        }//end switch



                        // end switch on the place the coin will go 
                        if (!coinSuspect)//Leave coin in the suspect folder if RAIDA is down
                        {
                            File.Delete(this.fileUtils.suspectFolder + suspectFileNames[j]);//Take the coin out of the suspect folder
                        }
                        else
                        {
                            this.fileUtils.writeTo(this.fileUtils.suspectFolder, detectedCC.cc);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Out.WriteLine("Not enough RAIDA were contacted to determine if the coin is authentic.");
                            Console.Out.WriteLine("Try again later.");
                            CoreLogger.Log("Not enough RAIDA were contacted to determine if the coin is authentic. Try again later.");
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

        public bool containsThreat(string pown)
        {
            bool threat = false;
            string doublePown = pown + pown;
            //There are four threat patterns that would allow attackers to seize other 
            //String UP_LEFT = "ff***f";
            //String UP_RIGHT = "ff***pf";
            //String DOWN_LEFT = "fp***ff";
            //String DOWN_RIGHT = "pf***ff";


            Match UP_LEFT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]fp", RegexOptions.IgnoreCase);
            Match UP_RIGHT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]pf", RegexOptions.IgnoreCase);
            Match DOWN_LEFT = Regex.Match(doublePown, @"fp[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);
            Match DOWN_RIGHT = Regex.Match(doublePown, @"pf[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);

            //Check if 
            if (UP_LEFT.Success || UP_RIGHT.Success || DOWN_LEFT.Success || DOWN_RIGHT.Success)
            {
                threat = true;
            }//end if coin contains threats.


            return threat;
        }//End Contains Threat
    }
}
