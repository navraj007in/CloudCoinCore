using System;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Founders
{
    public class MultiDetect
    {
        /*  INSTANCE VARIABLES */
        private RAIDA raida;
        private FileUtils fileUtils;



        /*  CONSTRUCTOR */
        public MultiDetect(FileUtils fileUtils)
        {
            raida = new RAIDA();
            this.fileUtils = fileUtils;
        }// end Detect constructor



        public int detectMulti(int detectTime, string receiptFile)
        {
            bool stillHaveSuspect = true;
            int coinNames = 0;

            while (stillHaveSuspect)
            {
                // LOAD ALL SUSPECT COIN NAMES IN AN ARRAY OF NAMES
                String[] suspectFileNames = new DirectoryInfo(this.fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder

                //CHECK TO SEE IF ANY OF THE FILES ARE ALREADY IN BANK. DELETE IF SO
                for (int i = 0; i < suspectFileNames.Length; i++)//for up to 200 coins in the suspect folder
                {

                    try
                    {
                        if (File.Exists(this.fileUtils.bankFolder + suspectFileNames[i]) || File.Exists(this.fileUtils.detectedFolder + suspectFileNames[i]))
                        {//Coin has already been imported. Delete it from import folder move to trash.
                            coinExists(suspectFileNames[i]);
                        }
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
                }// end for each coin to see if in bank


                //DUPLICATES HAVE BEEN DELETED, NOW DETECT
                suspectFileNames = new DirectoryInfo(this.fileUtils.suspectFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder


                //HOW MANY COINS WILL WE DETECT? LIMIT IT TO 200

                if (suspectFileNames.Length > 200)
                {
                    coinNames = 200;//do not detect more than 200 coins. 
                }
                else
                {
                    coinNames = suspectFileNames.Length;
                    stillHaveSuspect = false;// No need to get more names
                }

                //BUILD AN ARRAY OF COINS FROM THE FILE NAMES - UPTO 200
                CloudCoin[] cloudCoin = new CloudCoin[coinNames];
                CoinUtils[] cu = new CoinUtils[coinNames];
                Receipt receipt = createReceipt(coinNames, receiptFile);

                    for (int i = 0; i < coinNames; i++)//for up to 200 coins in the suspect folder
                {

                    try
                    {
                            cloudCoin[i] = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[i]);
                            cu[i] = new CoinUtils(cloudCoin[i]);
                            Console.Out.WriteLine("  Now scanning coin " + (i + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", cloudCoin[i].sn) + ", Denomination: " + cu[i].getDenomination());
                            CoreLogger.Log("  Now scanning coin " + (i + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", cloudCoin[i].sn) + ", Denomination: " + cu[i].getDenomination());
                        ReceitDetail detail = new ReceitDetail();
                        detail.sn = cloudCoin[i].sn;
                        detail.nn = cloudCoin[i].nn;
                        detail.status = "suspect";
                        detail.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        detail.note = "Waiting";
                        receipt.rd[i] = detail;
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


                //ALL COINS IN THE ARRAY, NOW DETECT

                CoinUtils[] detectedCC = raida.detectMultiCoin(cu, detectTime);

                //create receits
                using (StreamWriter sw = File.CreateText(fileUtils.receiptsFolder + receiptFile + ".json"))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(receipt));
                }


                    //Write the coins to the detected folder delete from the suspect
                    for (int c = 0; c < detectedCC.Length; c++)
                    {
                        fileUtils.writeTo(fileUtils.detectedFolder, detectedCC[c].cc);
                        File.Delete(fileUtils.suspectFolder + suspectFileNames[c]);//Delete the coin out of the suspect folder
                    }
            }//end while still have suspect
            return coinNames;
        }//End detectMulti All


        private void coinExists(String suspectFileName)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Out.WriteLine("  You tried to import a coin that has already been imported: " + suspectFileName);
            CoreLogger.Log("  You tried to import a coin that has already been imported: " + suspectFileName);
            File.Move(this.fileUtils.suspectFolder + suspectFileName, this.fileUtils.trashFolder + suspectFileName);
            Console.Out.WriteLine("  Suspect CloudCoin was moved to Trash folder.");
            CoreLogger.Log("  Suspect CloudCoin was moved to Trash folder.");
            Console.ForegroundColor = ConsoleColor.White;
        }//end coin exists

        Receipt createReceipt(int length, string id)
        {
            DateTime dt = DateTime.Now;
            TimeSpan tz = TimeZoneInfo.Local.GetUtcOffset(dt);
            string plus = "";
            if(tz > new TimeSpan(0))
            { plus = "+"; }
            else { plus = "-"; }
            Receipt receipt = new Receipt();
            receipt.time = dt.ToString("yyyy-MM-dd h:mm:tt");
            receipt.timezone = "UTC" + plus + tz.ToString("%h");
            receipt.bank_server = "localhost";
            receipt.total_authentic = 0;
            receipt.total_fracked = 0;
            receipt.total_counterfeit = 0;
            receipt.total_lost = 0;
            receipt.receipt_id = id;
            receipt.rd = new ReceitDetail[length];
            return receipt;
        }


    }//end class
}//end namespace
