using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;
using System.IO;

namespace WindowsFormsApp1
{
    class Banker : IBanker
    {
        public Banker(IFileUtils fileUtils) : base(fileUtils)
        {

        }

        public override int[] countCoins(string directoryPath)
        {
            int[] returnCounts = new int[6];
            // 0. Total, 1.1s, 2,5s, 3.25s 4.100s, 5.250s
            String[] fileNames = new DirectoryInfo(directoryPath).GetFiles().Select(o => o.Name).ToArray();
            for (int i = 0; (i < fileNames.Length); i++)
            {
                String[] nameParts = fileNames[i].Split('.');
                String denomination = nameParts[0];
                switch (denomination)
                {
                    case "1":
                        returnCounts[0]++;
                        returnCounts[1]++;
                        break;
                    case "5":
                        returnCounts[0] += 5;
                        returnCounts[2]++;
                        break;
                    case "25":
                        returnCounts[0] += 25;
                        returnCounts[3]++;
                        break;
                    case "100":
                        returnCounts[0] += 100;
                        returnCounts[4]++;
                        break;
                    case "250":
                        returnCounts[0] += 250;
                        returnCounts[5]++;
                        break;
                }
                // end switch
            }

            // end for each coin
            return returnCounts;
        }
    }
}
