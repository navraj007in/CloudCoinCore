using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using CloudCoinCore;

namespace WindowsFormsApp1
{
    public class FileUtils : IFileUtils
    {

        /* INSTANCE VARIABLES */

        /* CONSTRUCTOR */
        public FileUtils(String rootFolder, String importFolder, String importedFolder, String trashFolder, String suspectFolder, String frackedFolder, String bankFolder, String templateFolder, String counterfeitFolder, String directoryFolder, String exportFolder,String partialFolder) : base(rootFolder, importFolder,
            importedFolder, trashFolder, suspectFolder, frackedFolder, bankFolder, templateFolder, counterfeitFolder, directoryFolder, exportFolder,partialFolder)
        {
            //  initialise instance variables
            this.rootFolder = rootFolder;
            this.importFolder = importFolder;
            this.importedFolder = importedFolder;
            this.trashFolder = trashFolder;
            this.suspectFolder = suspectFolder;
            this.frackedFolder = frackedFolder;
            this.bankFolder = bankFolder;
            this.templateFolder = templateFolder;
            this.counterfeitFolder = counterfeitFolder;
            this.directoryFolder = directoryFolder;
            this.exportFolder = exportFolder;
            this.partialFolder = partialFolder;
        }  // End constructor


        /* PUBLIC METHODS */

        // This loads a JSON file (.stack) from the hard drive that contains only one CloudCoin and turns it into an object. 
        //   This uses Newton soft but causes a enrror System.IO.FileNotFoundException. Could not load file 'Newtonsoft.Json'  
        public override CloudCoin loadOneCloudCoinFromJsonFile(String loadFilePath)
        {

            CloudCoin returnCC = new CloudCoin();

            //Load file as JSON
            String incomeJson = this.importJSON(loadFilePath);
            //STRIP UNESSARY test
            int secondCurlyBracket = ordinalIndexOf(incomeJson, "{", 2) - 1;
            int firstCloseCurlyBracket = ordinalIndexOf(incomeJson, "}", 0) - secondCurlyBracket;
            // incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket);
            incomeJson = incomeJson.Substring(secondCurlyBracket, firstCloseCurlyBracket + 1);
            // Console.Out.WriteLine(incomeJson);
            //Deserial JSON

            try
            {
                returnCC = JsonConvert.DeserializeObject<CloudCoin>(incomeJson);

            }
            catch (JsonReaderException)
            {
                Console.WriteLine("There was an error reading files in your bank.");
                Console.WriteLine("You may have the aoid memo bug that uses too many double quote marks.");
                Console.WriteLine("Your bank files are stored using and older version that did not use properly formed JSON.");
                Console.WriteLine("Would you like to upgrade these files to the newer standard?");
                Console.WriteLine("Your files will be edited.");
                Console.WriteLine("1 for yes, 2 for no.");


            }

            return returnCC;
        }//end load one CloudCoin from JSON


        public override Stack loadManyCloudCoinFromJsonFile(String loadFilePath, string incomeJson)
        {

            Stack returnCC = JsonConvert.DeserializeObject<Stack>(incomeJson);


            return returnCC;
        }//end load one CloudCoin from JSON


        public override CloudCoin loadOneCloudCoinFromJPEGFile(String loadFilePath)
        {
            /* GET the first 455 bytes of he jpeg where the coin is located */
            String wholeString = "";
            byte[] jpegHeader = new byte[455];
            Console.Out.WriteLine("Load file path " + loadFilePath);
            FileStream fileStream = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read);
            try
            {
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(jpegHeader, sum, 455 - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            wholeString = bytesToHexString(jpegHeader);
            CloudCoin returnCC = this.parseJpeg(wholeString);
            // Console.Out.WriteLine("From FileUtils returnCC.fileName " + returnCC.fileName);
            return returnCC;
        }//end load one CloudCoin from JSON

        public override String importJSON(String jsonfile)
        {
            String jsonData = "";
            String line;

            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.

                using (StreamReader sr = new StreamReader(jsonfile, Encoding.Default, false, 2000))
                {
                    // Read and display lines from the file until the end of 
                    // the file is reached.
                    while (true)
                    {
                        line = sr.ReadLine();
                        if (line == null)
                        {
                            break;
                        }//End if line is null
                        jsonData = (jsonData + line + "\n");
                    }//end while true
                }//end using
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file " + jsonfile + " could not be read:");
                Console.WriteLine(e.Message);
            }
            return jsonData;
        }//end importJSON

        // en d json test
        public override String setJSON(CloudCoin cc)
        {
            const string quote = "\"";
            const string tab = "\t";
            String json = (tab + tab + "{ " + Environment.NewLine);// {
            json += tab + tab + quote + "nn" + quote + ":" + quote + cc.nn + quote + ", " + Environment.NewLine;// "nn":"1",
            json += tab + tab + quote + "sn" + quote + ":" + quote + cc.sn + quote + ", " + Environment.NewLine;// "sn":"367544",
            json += tab + tab + quote + "an" + quote + ": [" + quote;// "an": ["
            for (int i = 0; (i < 25); i++)
            {
                json += cc.an[i];// 8551995a45457754aaaa44
                if (i == 4 || i == 9 || i == 14 || i == 19)
                {
                    json += quote + "," + Environment.NewLine + tab + tab + tab + quote; //", 
                }
                else if (i == 24)
                {
                    // json += "\""; last one do nothing
                }
                else
                { // end if is line break
                    json += quote + ", " + quote;
                }

                // end else
            }// end for 25 ans

            json += quote + "]," + Environment.NewLine;//"],
            // End of ans
            CoinUtils cu = new CoinUtils(cc);
            cu.calcExpirationDate();
            json += tab + tab + quote + "ed" + quote + ":" + quote + cu.cc.ed + quote + "," + Environment.NewLine; // "ed":"9-2016",
            if (string.IsNullOrEmpty(cc.pown)) { cc.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu"; }//Set pown to unknow if it is not set. 
            json += tab + tab + quote + "pown" + quote + ":" + quote + cc.pown + quote + "," + Environment.NewLine;// "pown":"uuupppppffpppppfuuf",
            json += tab + tab + quote + "aoid" + quote + ": []" + Environment.NewLine;
            json += tab + tab + "}" + Environment.NewLine;
            // Keep expiration date when saving (not a truley accurate but good enought )
            return json;
        }
        // end get JSON

        /* Writes a JPEG To the Export Folder */
        public override bool writeJpeg(CloudCoin cc, string tag)
        {
            // Console.Out.WriteLine("Writing jpeg " + cc.sn);

            CoinUtils cu = new CoinUtils(cc);

            bool fileSavedSuccessfully = true;

            /* BUILD THE CLOUDCOIN STRING */
            String cloudCoinStr = "01C34A46494600010101006000601D05"; //THUMBNAIL HEADER BYTES
            for (int i = 0; (i < 25); i++)
            {
                cloudCoinStr = cloudCoinStr + cc.an[i];
            } // end for each an

            //cloudCoinStr += "204f42455920474f4420262044454645415420545952414e545320";// Hex for " OBEY GOD & DEFEAT TYRANTS "
            //cloudCoinStr += "20466f756e6465727320372d352d3137";// Founders 7-5-17
            cloudCoinStr += "4c6976652046726565204f7220446965";// Live Free or Die
            cloudCoinStr += "00000000000000000000000000";//Set to unknown so program does not export user data
                                                         // for (int i =0; i < 25; i++) {
                                                         //     switch () { }//end switch pown char
                                                         // }//end for each pown
            cloudCoinStr += "00"; // HC: Has comments. 00 = No
            cu.calcExpirationDate();
            cloudCoinStr += cu.edHex; // 01;//Expiration date Sep 2016 (one month after zero month)
            cloudCoinStr += "01";//  cc.nn;//network number
            String hexSN = cc.sn.ToString("X6");
            String fullHexSN = "";
            switch (hexSN.Length)
            {
                case 1: fullHexSN = ("00000" + hexSN); break;
                case 2: fullHexSN = ("0000" + hexSN); break;
                case 3: fullHexSN = ("000" + hexSN); break;
                case 4: fullHexSN = ("00" + hexSN); break;
                case 5: fullHexSN = ("0" + hexSN); break;
                case 6: fullHexSN = hexSN; break;
            }
            cloudCoinStr = (cloudCoinStr + fullHexSN);
            /* BYTES THAT WILL GO FROM 04 to 454 (Inclusive)*/
            byte[] ccArray = this.hexStringToByteArray(cloudCoinStr);


            /* READ JPEG TEMPLATE*/
            byte[] jpegBytes = null;
            switch (cu.getDenomination())
            {
                case 1: jpegBytes = readAllBytes(this.templateFolder + "jpeg1.jpg"); break;
                case 5: jpegBytes = readAllBytes(this.templateFolder + "jpeg5.jpg"); break;
                case 25: jpegBytes = readAllBytes(this.templateFolder + "jpeg25.jpg"); break;
                case 100: jpegBytes = readAllBytes(this.templateFolder + "jpeg100.jpg"); break;
                case 250: jpegBytes = readAllBytes(this.templateFolder + "jpeg250.jpg"); break;
            }// end switch


            /* WRITE THE SERIAL NUMBER ON THE JPEG */

            Bitmap bitmapimage;
            using (var ms = new MemoryStream(jpegBytes))
            {
                bitmapimage = new Bitmap(ms);
                // bitmapimage.Save(fileName2, ImageFormat.Jpeg);
            }
            Graphics graphics = Graphics.FromImage(bitmapimage);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            PointF drawPointAddress = new PointF(30.0F, 25.0F);
            graphics.DrawString(String.Format("{0:N0}", cc.sn) + " of 16,777,216 on Network: 1", new Font("Arial", 10), Brushes.White, drawPointAddress);

            ImageConverter converter = new ImageConverter();
            byte[] snBytes = (byte[])converter.ConvertTo(bitmapimage, typeof(byte[]));

            List<byte> b1 = new List<byte>(snBytes);
            List<byte> b2 = new List<byte>(ccArray);
            b1.InsertRange(4, b2);

            if (tag == "random")
            {
                Random r = new Random();
                int rInt = r.Next(100000, 1000000); //for ints
                tag = rInt.ToString();
            }

            string fileName = exportFolder + cu.fileName + tag + ".jpg";
            File.WriteAllBytes(fileName, b1.ToArray());
            Console.Out.WriteLine("Writing to " + fileName);
            return fileSavedSuccessfully;
        }//end write JPEG

        /* OPEN FILE AND READ ALL CONTENTS AS BYTE ARRAY */
        public override byte[] readAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                int fileLength = Convert.ToInt32(fs.Length);
                fs.Read(buffer, 0, fileLength);
            }
            return buffer;
        }//end read all bytes

        public override void overWrite(String folder, CloudCoin cc)
        {
            CoinUtils cu = new CoinUtils(cc);
            const string quote = "\"";
            const string tab = "\t";
            String wholeJson = "{" + Environment.NewLine; //{
            String json = this.setJSON(cc);

            wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
            wholeJson += json;
            wholeJson += Environment.NewLine + tab + "]" + Environment.NewLine + "}";

            File.WriteAllText(folder + cu.fileName + ".stack", wholeJson);
        }//End Overwrite

        public override bool writeTo(string folder, CloudCoin cc)
        {
            CoinUtils cu = new CoinUtils(cc);
            const string quote = "\"";
            const string tab = "\t";
            String wholeJson = "{" + Environment.NewLine; //{
            bool alreadyExists = true;
            String json = this.setJSON(cc);
            if (!File.Exists(folder + cu.fileName + ".stack"))
            {
                wholeJson += tab + quote + "cloudcoin" + quote + ": [" + Environment.NewLine; // "cloudcoin" : [
                wholeJson += json;
                wholeJson += Environment.NewLine + tab + "]" + Environment.NewLine + "}";
                File.WriteAllText(folder + cu.fileName + ".stack", wholeJson);
            }
            else
            {
                if (folder.Contains("Counterfeit") || folder.Contains("Trash"))
                {
                    //Let the program delete it
                    alreadyExists = false;
                    return alreadyExists;
                }
                else if (folder.Contains("Imported"))
                {
                    File.Delete(folder + cu.fileName + ".stack");
                    File.WriteAllText(folder + cu.fileName + ".stack", wholeJson);
                    alreadyExists = false;
                    return alreadyExists;
                }
                else
                {
                    Console.WriteLine(cu.fileName + ".stack" + " already exists in the folder " + folder);
                    return alreadyExists;

                }//end else

            }//File Exists
            File.WriteAllText(folder + cu.fileName + ".stack", wholeJson);
            alreadyExists = false;
            return alreadyExists;

        }//End Write To

        public override void CreateDirectoryStructure()
        {
            Directory.CreateDirectory(importFolder);
            Directory.CreateDirectory(importedFolder);
            Directory.CreateDirectory(trashFolder);
            Directory.CreateDirectory(suspectFolder);
            Directory.CreateDirectory(frackedFolder);
            Directory.CreateDirectory(bankFolder);

            Directory.CreateDirectory(templateFolder);
            Directory.CreateDirectory(counterfeitFolder);
            Directory.CreateDirectory(directoryFolder);
            Directory.CreateDirectory(exportFolder);
            Directory.CreateDirectory(partialFolder);

            //            Directory.CreateDirectory(languageFolder);

        }

        // en d json test


    }//Enc Class File Utils
}//End Namespace