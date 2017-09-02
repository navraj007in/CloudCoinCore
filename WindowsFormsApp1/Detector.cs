using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Detector : IDetector
    {
        RichTextBox txtLogs;

        public Detector(IFileUtils fileUtils, int timeout,RichTextBox txtLogs) : base(fileUtils, timeout)
        {
            this.txtLogs = txtLogs;
        }

        /*  PUBLIC METHODS */
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int[] detectAll()
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
                        txtLogs.AppendText("You tried to import a coin that has already been imported."+ Environment.NewLine);
                        File.Move(this.fileUtils.suspectFolder + suspectFileNames[j], this.fileUtils.trashFolder + suspectFileNames[j]);
                        Console.Out.WriteLine("Suspect CloudCoin was moved to Trash folder.");
                        txtLogs.AppendText("Suspect CloudCoin was moved to Trash folder."+ Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[j]);
                        CoinUtils cu = new CoinUtils(newCC);
                        Console.Out.WriteLine("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                        txtLogs.AppendText("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination()+ Environment.NewLine);
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
                            txtLogs.AppendText("Not enough RAIDA were contacted to determine if the coin is authentic. Try again later."+ txtLogs.Text);
                            Console.ForegroundColor = ConsoleColor.White;
                        }//end if else

                    }//end if file exists
                }
                catch (FileNotFoundException ex)
                {
                    Console.Out.WriteLine(ex);
                    txtLogs.AppendText(ex.ToString()+ Environment.NewLine);
                }
                catch (IOException ioex)
                {
                    Console.Out.WriteLine(ioex);

                    txtLogs.AppendText(ioex.ToString()+ Environment.NewLine);
                }// end try catch
            }// end for each coin to import
            results[0] = totalValueToBank;
            results[1] = totalValueToCounterfeit;
            results[2] = totalValueToFractured;
            results[3] = totalValueToKeptInSuspect;
            return results;
        }//Detect All

        public override void detectOne(int raida_id, int nn, int sn, String an, String pan, int d)
        {
            DetectionAgent da = new DetectionAgent(raida_id, 5000);
            responseArray[raida_id] = da.detect(nn, sn, an, pan, d);


        }//end detectOne

        public override CoinUtils partialDetectCoin(CoinUtils cu, int milliSecondsToTimeOut)
        {
            cu.generatePan();
            int[] echoes = (int[])RAIDA_Status.echoTime.Clone();
            int[] raidas = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            Array.Sort(echoes, raidas);
            Console.WriteLine("fastest raida: " + raidas[0] + "," + raidas[1] + "," + raidas[2] + "," + raidas[3] + "," + raidas[4] + "," + raidas[5] + "," + raidas[6] + "," + raidas[7] + "," + raidas[8] + "," + raidas[9] + "," + raidas[10] + "," + raidas[11] + "," + raidas[12] + "," + raidas[13] + "," + raidas[14] + "," + raidas[15]);
            var t00 = Task.Factory.StartNew(() => detectOne(raidas[00], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[00]], cu.pans[raidas[00]], cu.getDenomination()));
            var t01 = Task.Factory.StartNew(() => detectOne(raidas[01], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[01]], cu.pans[raidas[01]], cu.getDenomination()));
            var t02 = Task.Factory.StartNew(() => detectOne(raidas[02], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[02]], cu.pans[raidas[02]], cu.getDenomination()));
            var t03 = Task.Factory.StartNew(() => detectOne(raidas[03], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[03]], cu.pans[raidas[03]], cu.getDenomination()));
            var t04 = Task.Factory.StartNew(() => detectOne(raidas[04], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[04]], cu.pans[raidas[04]], cu.getDenomination()));
            var t05 = Task.Factory.StartNew(() => detectOne(raidas[05], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[05]], cu.pans[raidas[05]], cu.getDenomination()));
            var t06 = Task.Factory.StartNew(() => detectOne(raidas[06], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[06]], cu.pans[raidas[06]], cu.getDenomination()));
            var t07 = Task.Factory.StartNew(() => detectOne(raidas[07], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[07]], cu.pans[raidas[07]], cu.getDenomination()));
            var t08 = Task.Factory.StartNew(() => detectOne(raidas[08], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[08]], cu.pans[raidas[08]], cu.getDenomination()));
            var t09 = Task.Factory.StartNew(() => detectOne(raidas[09], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[09]], cu.pans[raidas[09]], cu.getDenomination()));
            var t10 = Task.Factory.StartNew(() => detectOne(raidas[10], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[10]], cu.pans[raidas[10]], cu.getDenomination()));
            var t11 = Task.Factory.StartNew(() => detectOne(raidas[11], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[11]], cu.pans[raidas[11]], cu.getDenomination()));
            var t12 = Task.Factory.StartNew(() => detectOne(raidas[12], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[12]], cu.pans[raidas[12]], cu.getDenomination()));
            var t13 = Task.Factory.StartNew(() => detectOne(raidas[13], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[13]], cu.pans[raidas[13]], cu.getDenomination()));
            var t14 = Task.Factory.StartNew(() => detectOne(raidas[14], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[14]], cu.pans[raidas[14]], cu.getDenomination()));
            var t15 = Task.Factory.StartNew(() => detectOne(raidas[15], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[15]], cu.pans[raidas[15]], cu.getDenomination()));
            //var t16 = Task.Factory.StartNew(() => detectOne(raidas[16], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[16]], cu.pans[raidas[16]], cu.getDenomination()));



            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            //Get data from the detection agents

            //nt k = 0;
            //for(int j =0; j<16;j++)
            //{
            //    if(responseArray[raidas[j]] != null && responseArray[raidas[j]].outcome == "error" && k < 9)
            //    {
            //        detectOne(raidas[16+k], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[16+k]], cu.pans[raidas[16+k]], cu.getDenomination());
            //       k++;
            //   }
            //}

            for (int i = 0; i < 25; i++)
            {
                if (responseArray[i] != null)
                {
                    cu.setPastStatus(responseArray[i].outcome, i);
                    txtLogs.AppendText(cu.cc.sn + " detect:" + i + " " + responseArray[i].fullResponse);
                }
                else
                {
                    cu.setPastStatus("undetected", i);
                };// should be pass, fail, error or undetected. 
            }//end for each detection agent

            cu.setAnsToPansIfPassed(true);
            cu.calculateHP();
            // cu.gradeCoin(); // sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            cu.calcExpirationDate();
            cu.grade();

            return cu;
        }//end detect coin
        public override int[] partialDetectAll()
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
                    if (File.Exists(this.fileUtils.partialFolder + suspectFileNames[j]))
                    {//Coin has already been imported. Delete it from import folder move to trash.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine("You tried to import a coin that has already been imported.");
                        txtLogs.AppendText("You tried to import a coin that has already been imported."+ Environment.NewLine);
                        File.Move(this.fileUtils.suspectFolder + suspectFileNames[j], this.fileUtils.trashFolder + suspectFileNames[j]);
                        Console.Out.WriteLine("Suspect CloudCoin was moved to Trash folder.");
                        txtLogs.AppendText("Suspect CloudCoin was moved to Trash folder."+Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        newCC = this.fileUtils.loadOneCloudCoinFromJsonFile(this.fileUtils.suspectFolder + suspectFileNames[j]);
                        CoinUtils cu = new CoinUtils(newCC);
                        Console.Out.WriteLine("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination());
                        txtLogs.AppendText("Now scanning coin " + (j + 1) + " of " + suspectFileNames.Length + " for counterfeit. SN " + string.Format("{0:n0}", newCC.sn) + ", Denomination: " + cu.getDenomination()+ Environment.NewLine);
                        Console.Out.WriteLine("");

                        CoinUtils detectedCC = this.raida.partialDetectCoin(cu, detectTime);
                        cu.calcExpirationDate();

                        if (j == 0)//If we are detecting the first coin, note if the RAIDA are working
                        {
                            for (int i = 0; i < 25; i++)// Checks any servers are down so we don't try to check them again. 
                            {
                                if (cu.getPastStatus(i) != "pass" && cu.getPastStatus(i) != "fail" && cu.getPastStatus(i) != "undetected")
                                {
                                    raida.raidaIsDetecting[i] = false;//Server is not working correctly, don't try it agian
                                }
                            }
                        }//end if it is the first coin we are detecting

                        cu.consoleReport();

                        bool alreadyExists = false;//Does the file already been imported?
                        switch (cu.getFolder().ToLower())
                        {
                            case "bank":
                                totalValueToBank++;
                                alreadyExists = this.fileUtils.writeTo(this.fileUtils.partialFolder, detectedCC.cc);
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
                            txtLogs.AppendText("Not enough RAIDA were contacted to determine if the coin is authentic. Try again later."+Environment.NewLine);
                            Console.ForegroundColor = ConsoleColor.White;
                        }//end if else

                    }//end if file exists
                }
                catch (FileNotFoundException ex)
                {
                    Console.Out.WriteLine(ex);
                    txtLogs.AppendText(ex.ToString());
                }
                catch (IOException ioex)
                {
                    Console.Out.WriteLine(ioex);
                    txtLogs.AppendText(ioex.ToString());
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
