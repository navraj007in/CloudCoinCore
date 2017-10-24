using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace Founders
{
    public class Exporter
    {
        /* INSTANCE VARIABLES */
        FileUtils fileUtils;
        

        /* CONSTRUCTOR */
        public Exporter(FileUtils fileUtils)
        {
            
            this.fileUtils = fileUtils;
        }

        /* PUBLIC METHODS */
        public void writeJPEGFiles(int m1, int m5, int m25, int m100, int m250, String tag)
        {
            int totalSaved = m1 + (m5 * 5) + (m25 * 25) + (m100 * 100) + (m250 * 250);// Total value of all coins
            int coinCount = m1 + m5 + m25 + m100 + m250; // Total number of coins 
            String[] coinsToDelete = new String[coinCount];
            String[] bankedFileNames = new DirectoryInfo(this.fileUtils.bankFolder).GetFiles().Select(o => o.Name).ToArray(); // list all file names with bank extension
            String[] frackedFileNames = new DirectoryInfo(this.fileUtils.frackedFolder).GetFiles().Select(o => o.Name).ToArray(); // list all file names with bank extension
            String[] partialFileNames = new DirectoryInfo(this.fileUtils.partialFolder).GetFiles().Select(o => o.Name).ToArray();

            var list = new List<string>();
            list.AddRange(bankedFileNames);
            list.AddRange(frackedFileNames);
            list.AddRange(partialFileNames);

            bankedFileNames = list.ToArray(); // Add the two arrays together

            String path = this.fileUtils.exportFolder;//the word path is shorter than other stuff

            // Look at all the money files and choose the ones that are needed.
            for (int i = 0; i < bankedFileNames.Length; i++)
            {
                String bankFileName = (this.fileUtils.bankFolder + bankedFileNames[i]);
                String frackedFileName = (this.fileUtils.frackedFolder + bankedFileNames[i]);
                String partialFileName = (this.fileUtils.partialFolder + bankedFileNames[i]);

                // Get denominiation
                String denomination = bankedFileNames[i].Split('.')[0];
                try
                {
                    switch (denomination)
                    {
                        case "1":
                            if (m1 > 0)
                            {
                                this.jpegWriteOne(path, tag, bankFileName, frackedFileName, partialFileName); m1--;
                            }
                            break;
                        case "5":
                            if (m5 > 0)
                            {

                                this.jpegWriteOne(path, tag, bankFileName, frackedFileName, partialFileName); m5--;
                            }
                            break;
                        case "25":
                            if (m25 > 0)
                            {

                                this.jpegWriteOne(path, tag, bankFileName, frackedFileName, partialFileName); m25--;
                            }
                            break;

                        case "100":
                            if (m100 > 0)
                            {
                                this.jpegWriteOne(path, tag, bankFileName, frackedFileName, partialFileName); m100--;
                            }
                            break;

                        case "250":
                            if (m250 > 0)
                            { this.jpegWriteOne(path, tag, bankFileName, frackedFileName, partialFileName); m250--; }
                            break;
                    }//end switch

                    if (m1 == 0 && m5 == 0 && m25 == 0 && m100 == 0 && m250 == 0)// end if file is needed to write jpeg
                    {
                        break;// Break if all the coins have been called for.
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
                }//end catch 
            }// for each 1 note  
        }//end write all jpegs

        /* Write JSON to .stack File  */
        public bool writeJSONFile(int m1, int m5, int m25, int m100, int m250, String tag)
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
            String json = "{" + Environment.NewLine;
            json = json + "\t\"cloudcoin\": " + Environment.NewLine;
            json = json + "\t[" + Environment.NewLine;
            String bankFileName;
            String frackedFileName;
            String partialFileName;
            string denomination;

            // Put all the JSON together and add header and footer
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
                        CloudCoin coinNote = fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                        coinNote.aoid = null;//Clear all owner data
                        json = json + fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        coinNote.aoid = null;//Clear all owner data
                        json = json + fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = partialFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        coinNote.aoid = null;
                        json = json + this.fileUtils.setJSON(coinNote);
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
                        coinNote.aoid = null;//Clear all owner data
                        json = json + this.fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        coinNote.aoid = null;//Clear all owner data
                        json = json + fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = partialFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        coinNote.aoid = null;
                        json = json + this.fileUtils.setJSON(coinNote);
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
                        coinNote.aoid = null;//Clear all owner data
                        json = json + this.fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        coinNote.aoid = null;//Clear all owner data
                        json = json + fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = partialFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        coinNote.aoid = null;
                        json = json + this.fileUtils.setJSON(coinNote);
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
                        coinNote.aoid = null;//Clear all owner data
                        json = json + this.fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        coinNote.aoid = null;//Clear all owner data
                        json = json + fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = partialFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        coinNote.aoid = null;
                        json = json + this.fileUtils.setJSON(coinNote);
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
                        coinNote.aoid = null;//Clear all owner data
                        json = json + this.fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = bankFileName;
                        c++;
                    }
                    else if (File.Exists(partialFileName)) // Is it a partial file 
                    {
                        CloudCoin coinNote = fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                        coinNote.aoid = null;//Clear all owner data
                        json = json + fileUtils.setJSON(coinNote);
                        coinsToDelete[c] = partialFileName;
                        c++;
                    }
                    else
                    {
                        CloudCoin coinNote = this.fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                        coinNote.aoid = null;
                        json = json + this.fileUtils.setJSON(coinNote);
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

            /*WRITE JSON TO FILE*/
            json = json + "\t] " + Environment.NewLine;
            json += "}";
            String filename = (this.fileUtils.exportFolder + totalSaved + ".CloudCoins." + tag + ".stack");
            if (File.Exists(filename))
            {
                // tack on a random number if a file already exists with the same tag
                Random rnd = new Random();
                int tagrand = rnd.Next(999);
                filename = (this.fileUtils.exportFolder + Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + tagrand + ".stack");
            }//end if file exists

            File.WriteAllText(filename, json);
            Console.Out.WriteLine("Writing to : ");
            CoreLogger.Log("Writing to : " + filename);
            Console.Out.WriteLine(filename);
            /*DELETE FILES THAT HAVE BEEN EXPORTED*/
            for (int cc = 0; cc < coinsToDelete.Length; cc++)
            {
                // Console.Out.WriteLine("Deleting " + coinsToDelete[cc]);
                if (coinsToDelete[cc] != null) { File.Delete(coinsToDelete[cc]); }
            }//end for all coins to delete

            // end if write was good
            return jsonExported;
        }//end write json to file


        /* PRIVATE METHODS */
        private void jpegWriteOne(String path, String tag, String bankFileName, String frackedFileName, String partialFileName)
        {
            if (File.Exists(bankFileName))//If the file is a bank file, export a good bank coin
            {
                CloudCoin jpgCoin = this.fileUtils.loadOneCloudCoinFromJsonFile(bankFileName);
                if (this.fileUtils.writeJpeg(jpgCoin, tag))//If the jpeg writes successfully 
                {
                    File.Delete(bankFileName);//Delete the files if they have been written to
                }//end if write was good. 
            }
            else if (File.Exists(partialFileName))//If the file is a bank file, export a good bank coin
            {
                CloudCoin jpgCoin = this.fileUtils.loadOneCloudCoinFromJsonFile(partialFileName);
                if (this.fileUtils.writeJpeg(jpgCoin, tag))//If the jpeg writes successfully 
                {
                    File.Delete(partialFileName);//Delete the files if they have been written to
                }//end if write was good. 
            }
            else//Export a fracked coin. 
            {
                CloudCoin jpgCoin = fileUtils.loadOneCloudCoinFromJsonFile(frackedFileName);
                if (this.fileUtils.writeJpeg(jpgCoin, tag))
                {
                    File.Delete(frackedFileName);//Delete the files if they have been written to
                }//end if
            }//end else
        }//End write one jpeg 
    }// end exporter class
}//end namespace
