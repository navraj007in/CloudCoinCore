using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Founders
{
     public class MultiDetect
    {
        /*  INSTANCE VARIABLES */
        private RAIDA raida;
        private FileUtils fileUtils;
        private int detectTime;


        /*  CONSTRUCTOR */
        public MultiDetect(FileUtils fileUtils, int timeout)
        {
            raida = new RAIDA(timeout);
            this.fileUtils = fileUtils;
            detectTime = timeout;
        }// end Detect constructor


  
        public int detectMulti()
        {
            // LOAD THE .suspect COINS ONE AT A TIME AND TEST THEM
            String[] suspectFileNames = new DirectoryInfo(this.fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder

            int CoinsToDetect = 0;
            if (suspectFileNames.Length > 200)
            {
                CoinsToDetect = 200;//do not detect more than 200 coins. 
            }else{
                CoinsToDetect = suspectFileNames.Length;//do not detect more than 200 coins. 
            }

            CloudCoin[] newCCs = new CloudCoin[CoinsToDetect];
            CoinUtils[] cu = new CoinUtils[CoinsToDetect];


            for (int i = 0; i < CoinsToDetect; i++)//for up to 200 coins in the suspect folder
            {

                try
                {
                    //Do not test files that already exist n the Bank or in the Detected folders. 
                    if ( File.Exists(this.fileUtils.bankFolder + suspectFileNames[i]) || File.Exists(this.fileUtils.detectedFolder + suspectFileNames[i]))
                    {//Coin has already been imported. Delete it from import folder move to trash.
                        coinExists(suspectFileNames[i]);
                    }
                    else
                    {
                        newCCs[i] = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[i]);
                        cu[i] = new CoinUtils(newCCs[i]);

                        Console.Out.WriteLine("  Now scanning coin " + (i + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCCs[i].sn) + ", Denomination: " + cu[i].getDenomination());
                        CoreLogger.Log("  Now scanning coin " + (i + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCCs[i].sn) + ", Denomination: " + cu[i].getDenomination());
                        Console.Out.WriteLine("");
                    }//End else is not already in bank folder

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
 
            //Write to Receipt/ Purchase order to Folder here. 

            CoinUtils[] detectedCC = raida.detectMultiCoin(cu, detectTime);

            //Write the coins to the detected folder delete from the suspect
            for (int c=0; c < detectedCC.Length; c++)
            {
                detectedCC[c].consoleReport();
            }

        }//End detectMulti All


        public void coinExists(String suspectFileName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Out.WriteLine("  You tried to import a coin that has already been imported.");
            CoreLogger.Log("  You tried to import a coin that has already been imported.");
            File.Move(this.fileUtils.suspectFolder + suspectFileName, this.fileUtils.trashFolder + suspectFileName);
            Console.Out.WriteLine("  Suspect CloudCoin was moved to Trash folder.");
            CoreLogger.Log("  Suspect CloudCoin was moved to Trash folder.");
            Console.ForegroundColor = ConsoleColor.White;
        }//end coin exists


    }//end class
}//end namespace
