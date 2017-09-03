using System;
using System.Net;
using System.Linq;
using System.IO;
using System.Collections.Specialized;

namespace Founders
{
    class Emailer
    {
        //fields
        //constructor
        public Emailer() { }
        //methods

        public void email(string email_address, string bankFolder, string frackedFolder)
        {
            //Get array of SN from Bank and Fracked
            String[] bankFileNames = new DirectoryInfo(bankFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            String[] frackedFileNames = new DirectoryInfo(frackedFolder).GetFiles().Select(o => o.Name).ToArray();//Get all files in suspect folder
            
            //if empty do not send email.
            if (bankFileNames.Length < 1 && frackedFileNames.Length < 1) { return; }

            //extract SNs from file names. 
            string sns = "";
            int count = 0;//for tracking comma
            for ( int i = 0; i < bankFileNames.Length; i++) {
                string[] nameArray;
                nameArray = bankFileNames[i].Split('.');
                if (count != 0) { sns += ","; }
                sns += nameArray[3];
            }//end for each coin
            for (int i = 0; i < frackedFileNames.Length ; i++)
            {
                string[] nameArray;
                nameArray = bankFileNames[i].Split('.');
                if (count != 0) { sns += ","; }
                sns += nameArray[3];
            }//end for each coin




            //Post array to: 
            //   public static void AddArray(this WebClient webClient, string key, params string[] values)
            //  {
            //      int index = webClient.QueryString.Count;

            //      foreach (string value in values)
            //      {
            //          webClient.QueryString.Add(key + "[" + index + "]", value);
            //         index++;
            //      }
            // }



            //using (WebClient client = new WebClient())
            //{
            //    int index = client.QueryString.Count;
            //    string key = "sn";
            //    for (int i = 0; i < frackedFileNames.Length; i++)
            //    {
            //        client.QueryString.Add(key + "[" + index + "]", value);
            //        index++;
            //    }
            //    for (int i = 0; i < bankFileNames.Length; i++)
            //    {
            //        client.QueryString.Add(key + "[" + index + "]", value);
            //        index++;
            //    }



            

        }//End email

    }
}
