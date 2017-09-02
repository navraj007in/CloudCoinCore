using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;


namespace CloudCoinCore
{
    public abstract class IFileUtils
    {
        public String rootFolder;
        public String importFolder;
        public String importedFolder;
        public String trashFolder;
        public String suspectFolder;
        public String frackedFolder;
        public String bankFolder;
        public String templateFolder;
        public String counterfeitFolder;
        public String directoryFolder;
        public String exportFolder;

        /* CONSTRUCTOR */
        public IFileUtils(String rootFolder, String importFolder, String importedFolder, String trashFolder, String suspectFolder, String frackedFolder, String bankFolder, String templateFolder, String counterfeitFolder, String directoryFolder, String exportFolder)
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
        }  // End constructor

        /* PUBLIC METHODS */

        // This loads a JSON file (.stack) from the hard drive that contains only one CloudCoin and turns it into an object. 
        //   This uses Newton soft but causes a enrror System.IO.FileNotFoundException. Could not load file 'Newtonsoft.Json'  
        public abstract CloudCoin loadOneCloudCoinFromJsonFile(String loadFilePath);

        public abstract void CreateDirectoryStructure();

        public abstract Stack loadManyCloudCoinFromJsonFile(String loadFilePath, string incomeJson);


        public abstract CloudCoin loadOneCloudCoinFromJPEGFile(String loadFilePath);

        public abstract String importJSON(String jsonfile);
        // en d json test
        public abstract String setJSON(CloudCoin cc);
        // end get JSON
        public abstract bool writeTo(String folder, CloudCoin cc);

        /* Writes a JPEG To the Export Folder */
        public abstract bool writeJpeg(CloudCoin cc, string tag);

        /* OPEN FILE AND READ ALL CONTENTS AS BYTE ARRAY */
        public abstract byte[] readAllBytes(string fileName);

        public abstract void overWrite(String folder, CloudCoin cc);

        public CloudCoin parseJpeg(String wholeString)
        {

            CloudCoin cc = new CloudCoin();
            int startAn = 40;
            for (int i = 0; i < 25; i++)
            {

                cc.an[i] = wholeString.Substring(startAn, 32);
                // Console.Out.WriteLine(i +": " + cc.an[i]);
                startAn += 32;
            }

            // end for
            cc.aoid = null;
            // wholeString.substring( 840, 895 );
            //cc.hp = 25;
            // Integer.parseInt(wholeString.substring( 896, 896 ), 16);
            cc.ed = wholeString.Substring(898, 4);
            cc.nn = Convert.ToInt32(wholeString.Substring(902, 2), 16);
            cc.sn = Convert.ToInt32(wholeString.Substring(904, 6), 16);
            cc.pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";
            //  Console.Out.WriteLine("parseJpeg cc.fileName " + cc.fileName);
            return cc;
        }// end parse Jpeg

        // en d json test
        public byte[] hexStringToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }//End hex string to byte array

        public int ordinalIndexOf(String str, String substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
                pos = str.IndexOf(substr, pos + 1);
            return pos;
        }

        public string bytesToHexString(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int length = data.Length;
            char[] hex = new char[length * 2];
            int num1 = 0;
            for (int index = 0; index < length * 2; index += 2)
            {
                byte num2 = data[num1++];
                hex[index] = GetHexValue(num2 / 0x10);
                hex[index + 1] = GetHexValue(num2 % 0x10);
            }
            return new string(hex);
        }//End NewConverted

        private char GetHexValue(int i)
        {
            if (i < 10)
            {
                return (char)(i + 0x30);
            }
            return (char)((i - 10) + 0x41);
        }//end GetHexValue


    }//Enc Class File Utils


}
