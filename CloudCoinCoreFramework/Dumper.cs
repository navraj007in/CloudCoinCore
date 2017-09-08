using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Founders
{
    public class Dumper
    {
        FileUtils fileUtils;
        Random random;
        

        public Dumper(FileUtils fileUtils)
        {
            
            this.fileUtils = fileUtils;
            random = new Random();
        }//end Dumper constructor

        public void dumpAll()
        {
            // 1. Get all file names in bank
            String[] bankFileNames = new DirectoryInfo(this.fileUtils.bankFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder

            // 2. loop. Each name create a new name
            String newFileName = "";
            for (int i = 0; i < bankFileNames.Length; i++)
            {
                newFileName = bankFileNames[i];
                string randomIntString = "." +  random.Next(100000, 10000000) .ToString() + ".";
                newFileName = newFileName.Replace("..", randomIntString);

                // 3. move file to export. 
                File.Move(this.fileUtils.bankFolder + bankFileNames[i], this.fileUtils.exportFolder + newFileName);

            }//end for each file name
        }//end dump all


        /* Write JSON to .stack File  */
        public bool dumpSome(int m1, int m5, int m25, int m100, int m250)
        {
            bool jsonExported = true;
            int totalSaved = m1 + (m5 * 5) + (m25 * 25) + (m100 * 100) + (m250 * 250);
            // Track the total coins
            int coinCount = m1 + m5 + m25 + m100 + m250;
            String[] coinsToDelete = new String[coinCount];
            String[] bankedFileNames = new DirectoryInfo(this.fileUtils.bankFolder).GetFiles().Select(o => o.Name).ToArray();//Get all names in bank folder
            String[] frackedFileNames = new DirectoryInfo(this.fileUtils.frackedFolder).GetFiles().Select(o => o.Name).ToArray(); ;
            String[] partialFileNames = new DirectoryInfo(this.fileUtils.partialFolder).GetFiles().Select(o => o.Name).ToArray();
            // Add the two arrays together
            var list = new List<String>();
            list.AddRange(bankedFileNames);
            list.AddRange(frackedFileNames);
            list.AddRange(partialFileNames);

            // Program will spend fracked files like perfect files
            bankedFileNames = list.ToArray();


            // Check to see the denomination by looking at the file start
            int c = 0;
            // c= counter

            String json = "";
            String start = "{" + Environment.NewLine;
            start = start + "\t\"cloudcoin\": [" + Environment.NewLine;
            String end = "\t]" + Environment.NewLine + "}";

            String bankFileName;
            String frackedFileName;
            String partialFileName;
            string denomination;

            // Put all the JSON together and add header and footer
            String filename = "";
            for (int i = 0; (i < bankedFileNames.Length); i++)
            {
                denomination = bankedFileNames[i].Split('.')[0];
                bankFileName = this.fileUtils.bankFolder + bankedFileNames[i];//File name in bank folder
                frackedFileName = this.fileUtils.frackedFolder + bankedFileNames[i];//File name in fracked folder
                partialFileName = this.fileUtils.partialFolder + bankedFileNames[i];
                if (denomination == "1" && m1 > 0)
                {
                    if (c != 0)//This is the json seperator between each coin. It is not needed on the first coin
                    {
                        json += ",\n";
                    }

                    if (File.Exists(bankFileName)) // Is it a bank file 
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = frackedFileName;
                        c++;
                    }

                    m1--;
                    // Get the clean JSON of the coin
                }// end if coin is a 1

                if (denomination == "5" && m5 > 0)
                {
                    if ((c != 0))
                    {
                        json += ",\n";
                    }

                    if (File.Exists(bankFileName))
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = frackedFileName;
                        c++;
                    }

                    m5--;
                } // end if coin is a 5

                if (denomination == "25" && m25 > 0)
                {
                    if ((c != 0))
                    {
                        json += ",\n";
                    }

                    if (File.Exists(bankFileName))
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = frackedFileName;
                        c++;
                    }

                    m25--;
                }// end if coin is a 25

                if (denomination == "100" && m100 > 0)
                {
                    if ((c != 0))
                    {
                        json += ",\n";
                    }

                    if (File.Exists(bankFileName))
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = frackedFileName;
                        c++;
                    }

                    m100--;
                } // end if coin is a 100

                if (denomination == "250" && m250 > 0)
                {
                    if ((c != 0))
                    {
                        json += ",\n";
                    }

                    if (File.Exists(bankFileName))
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;//Clear all owner data
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." + random.Next(100000, 10000000) + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        CoinUtils cu = new CoinUtils(coinNote);
                        coinNote.aoid = null;
                        coinNote.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
                        json = start + this.fileUtils.setJSON(coinNote) + end;
                        filename = this.fileUtils.exportFolder + Path.DirectorySeparatorChar + cu.getDenomination() + ".CloudCoins." +  random.Next(100000, 10000000)  + ".stack";
                        File.WriteAllText(filename, json);
                        Console.Out.WriteLine("Writing to " + filename);
                        CoreLogger.Log("Writing to " + filename);
                        coinsToDelete[c] = frackedFileName;
                        c++;
                    }

                    m250--;
                }// end if coin is a 250

                if (m1 == 0 && m5 == 0 && m25 == 0 && m100 == 0 && m250 == 0)
                {
                    break;
                } // Break if all the coins have been called for.     
            }// end for each coin needed

            /*DELETE FILES THAT HAVE BEEN EXPORTED*/
            for (int cc = 0; cc < coinsToDelete.Length; cc++)
            {
                // Console.Out.WriteLine("Deleting " + coinsToDelete[cc]);
                if (coinsToDelete[cc] != null) { File.Delete(coinsToDelete[cc]); }
            }//end for all coins to delete

            // end if write was good
            return jsonExported;
        }//end write json to file

    }//end class dump
}//end namespace
