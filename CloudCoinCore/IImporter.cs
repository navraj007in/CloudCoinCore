using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinCore
{
    public abstract class IImporter
    {
        IFileUtils fileUtils;

        /* CONSTRUCTOR */
        public IImporter(IFileUtils fileUtils)
        {
            this.fileUtils = fileUtils;
        }//Constructor

        /* PUBLIC METHODS */
        public abstract bool importAll();


        /* PRIVATE METHODS */

        /* IMPORT ONE FILE. COULD BE A JPEG, STACK or CHEST */
        protected abstract bool importOneFile(String fname);

        /* IMPORT ONE JPEG */
        protected abstract bool importJPEG(String fileName);
        //Move one jpeg to suspect folder. 

        /* IMPORT ONE STACK FILE */
        protected abstract bool importStack(String fileName);

        protected abstract bool seemsValidJSON(string json);

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }//End is odd

        public static bool IsNotFive(int value)
        {
            return value % 5 != 0;
        }//End is not five

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

        protected CloudCoin parseJpeg(String wholeString)
        {

            int startAn = 40;
            List<string> ans = new List<string>();
            for (int i = 0; i < 25; i++)
            {
                ans.Add(wholeString.Substring(startAn, 32)); // Console.Out.WriteLine(i +": " + cc.ans[i]);
                startAn += 32;
            } // end for each AN
            List<string> aoid = new List<string>();
            string pown = "uuuuuuuuuuuuuuuuuuuuuuuuu";//Don't trust the incomming jpeg
            string ed = null;//Don't trust the incomming jpeg
            int nn = Convert.ToInt32(wholeString.Substring(902, 2), 16);
            int sn = Convert.ToInt32(wholeString.Substring(904, 6), 16);

            CloudCoin cc = new CloudCoin(nn, sn, ans, ed, pown, aoid);
            return cc;
        }// end parse Jpeg

        public abstract void moveFile(string fromPath, string tooPath);

    }
}
