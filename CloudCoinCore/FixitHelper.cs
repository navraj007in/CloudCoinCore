using System;

namespace Founders
{
    public class FixitHelper
    {
        //  instance variables
        public int[] trustedServers = new int[8];

        // Each servers only trusts eight others
        public int[] trustedTriad1;
        public int[] trustedTriad2;
        public int[] trustedTriad3;
        public int[] trustedTriad4;
        public int[] currentTriad;
        public String[] ans1;
        public String[] ans2;
        public String[] ans3;
        public String[] ans4;
        public String[] currentAns = new String[4];
        public bool finnished = false;

        public bool triad_1_is_ready = false;
        public bool triad_2_is_ready = false;
        public bool triad_3_is_ready = false;
        public bool triad_4_is_ready = false;
        public bool currentTriadReady = false;

        // All triads have been tried
        public FixitHelper(int raidaNumber, String[] ans)
        {
            // Create an array so we can make sure all the servers submitted are within this allowabel group of servers.
            int[] trustedServers = new int[] { 33, 33, 33, 33, 33, 33, 33, 33 };  // bogus data to make sure initializtion happened
            // FIND TRUSTED NEIGHBOURS
            switch (raidaNumber)
            {
                case 0: this.trustedServers = new int[] { 19, 20, 21, 24, 1, 4, 5, 6 }; break;
                case 1: this.trustedServers = new int[] { 20, 21, 22, 0, 2, 5, 6, 7 }; break;
                case 2: this.trustedServers = new int[] { 21, 22, 23, 1, 3, 6, 7, 8 }; break;
                case 3: this.trustedServers = new int[] { 22, 23, 24, 2, 4, 7, 8, 9 }; break;
                case 4: this.trustedServers = new int[] { 23, 24, 0, 3, 5, 8, 9, 10 }; break;
                case 5: this.trustedServers = new int[] { 24, 0, 1, 4, 6, 9, 10, 11 }; break;
                case 6: this.trustedServers = new int[] { 0, 1, 2, 5, 7, 10, 11, 12 }; break;
                case 7: this.trustedServers = new int[] { 1, 2, 3, 6, 8, 11, 12, 13 }; break;
                case 8: this.trustedServers = new int[] { 2, 3, 4, 7, 9, 12, 13, 14 }; break;
                case 9: this.trustedServers = new int[] { 3, 4, 5, 8, 10, 13, 14, 15 }; break;
                case 10: this.trustedServers = new int[] { 4, 5, 6, 9, 11, 14, 15, 16 }; break;
                case 11: this.trustedServers = new int[] { 5, 6, 7, 10, 12, 15, 16, 17 }; break;
                case 12: this.trustedServers = new int[] { 6, 7, 8, 11, 13, 16, 17, 18 }; break;
                case 13: this.trustedServers = new int[] { 7, 8, 9, 12, 14, 17, 18, 19 }; break;
                case 14: this.trustedServers = new int[] { 8, 9, 10, 13, 15, 18, 19, 20 }; break;
                case 15: this.trustedServers = new int[] { 9, 10, 11, 14, 16, 19, 20, 21 }; break;
                case 16: this.trustedServers = new int[] { 10, 11, 12, 15, 17, 20, 21, 22 }; break;
                case 17: this.trustedServers = new int[] { 11, 12, 13, 16, 18, 21, 22, 23 }; break;
                case 18: this.trustedServers = new int[] { 12, 13, 14, 17, 19, 22, 23, 24 }; break;
                case 19: this.trustedServers = new int[] { 13, 14, 15, 18, 20, 23, 24, 0 }; break;
                case 20: this.trustedServers = new int[] { 14, 15, 16, 19, 21, 24, 0, 1 }; break;
                case 21: this.trustedServers = new int[] { 15, 16, 17, 20, 22, 0, 1, 2 }; break;
                case 22: this.trustedServers = new int[] { 16, 17, 18, 21, 23, 1, 2, 3 }; break;
                case 23: this.trustedServers = new int[] { 17, 18, 19, 22, 24, 2, 3, 4 }; break;
                case 24: this.trustedServers = new int[] { 18, 19, 20, 23, 0, 3, 4, 5 }; break;
            }
            // end switch raidaNumber
            this.trustedTriad1 = new int[] { this.trustedServers[0], this.trustedServers[1], this.trustedServers[3] };
            this.trustedTriad2 = new int[] { this.trustedServers[1], this.trustedServers[2], this.trustedServers[4] };
            this.trustedTriad3 = new int[] { this.trustedServers[3], this.trustedServers[5], this.trustedServers[6] };
            this.trustedTriad4 = new int[] { this.trustedServers[4], this.trustedServers[6], this.trustedServers[7] };
            this.currentTriad = this.trustedTriad1;
            // Try the first tried first

            ans1 = new String[] { ans[trustedTriad1[0]], ans[trustedTriad1[1]], ans[trustedTriad1[2]] };
            ans2 = new String[] { ans[trustedTriad2[0]], ans[trustedTriad2[1]], ans[trustedTriad2[2]] };
            ans3 = new String[] { ans[trustedTriad3[0]], ans[trustedTriad3[1]], ans[trustedTriad3[2]] };
            ans4 = new String[] { ans[trustedTriad4[0]], ans[trustedTriad4[1]], ans[trustedTriad4[2]] };

            currentAns = ans1;
        }// end of constructor


        public void setCornerToCheck(int corner)
        {
            switch (corner)
            {
                case 1:
                    this.currentTriad = this.trustedTriad1;
                    currentTriadReady = triad_1_is_ready;
                    break;
                case 2:
                    this.currentTriad = this.trustedTriad2;
                    currentTriadReady = triad_2_is_ready;
                    break;
                case 3:
                    this.currentTriad = this.trustedTriad3;
                    currentTriadReady = triad_3_is_ready;
                    break;
                case 4:
                    this.currentTriad = this.trustedTriad4;
                    currentTriadReady = triad_4_is_ready;
                    break;
                default:
                    this.finnished = true;
                    break;
            }
            // end switch
        }//end set corner to check

        /***
     * This changes the Triads that will be used
     */
        public void setCornerToTest(int mode)
        {

            switch (mode)
            {
                case 1:
                    currentTriad = trustedTriad1;
                    currentAns = ans1;
                    currentTriadReady = true;
                    break;
                case 2:
                    currentTriad = trustedTriad2;
                    currentAns = ans2;
                    currentTriadReady = true;
                    break;
                case 3:
                    currentTriad = trustedTriad3;
                    currentAns = ans3;
                    currentTriadReady = true;
                    break;
                case 4:
                    currentTriad = trustedTriad4;
                    currentAns = ans4;
                    currentTriadReady = true;
                    break;
                default:
                    this.finnished = true;
                    break;
            }//end switch
        }//End fix Guid

    }
}
