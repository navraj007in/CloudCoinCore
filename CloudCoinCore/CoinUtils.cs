using PCLCrypto;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Founders
{
    public class CoinUtils
    {
        //  instance variables
        public CloudCoin cc;
        public String[] pans = new String[25];// Proposed Authenticty Numbers
        public string pastPown = "uuuuuuuuuuuuuuuuuuuuuuuuu";//Used to see if there are any improvments in defracking
        public String edHex;// Months from zero date that the coin will expire. 
        public int hp;// HitPoints (1-25, One point for each server not failed)
        public String fileName;
        public String json;
        public byte[] jpeg;
        public const int YEARSTILEXPIRE = 2;
        public enum Folder { Suspect, Counterfeit, Fracked, Bank, Trash, Detected, Lost, Dangerous };
        public Folder folder;
        public String[] gradeStatus = new String[3];// What passed, what failed, what was undetected


        //CONSTRUCTORS
        public CoinUtils(CloudCoin cc)
        {
            //  initialise instance variables
            this.cc = cc;
            for (int i = 0; i < 25; i++) { pans[i] = this.generatePan(); } // end for each pan
            edHex = "FF";//Max allowed. 
            hp = 25;//Max allowed
            fileName = this.getDenomination() + ".CloudCoin." + cc.nn + "." + cc.sn + ".";
            json = "";
            jpeg = null;
        }//end constructor


        //METHODS

        public CoinUtils()
        {

        }//end constructor

        public string getPastStatus(int raida_id)
        {
            string returnString = "";
            char[] pownArray = cc.pown.ToCharArray();
            switch (pownArray[raida_id])
            {
                case 'e': returnString = "error"; break;
                case 'f': returnString = "fail"; break;
                case 'p': returnString = "pass"; break;
                case 'u': returnString = "undetected"; break;
                case 'n': returnString = "noresponse"; break;
            }//end switch
            return returnString;
        }//end getPastStatus

        public bool setPastStatus(string status, int raida_id)
        {

            char[] pownArray = cc.pown.ToCharArray();
            switch (status)
            {
                case "error": pownArray[raida_id] = 'e'; break;
                case "fail": pownArray[raida_id] = 'f'; break;
                case "pass": pownArray[raida_id] = 'p'; break;
                case "undetected": pownArray[raida_id] = 'u'; break;
                case "noresponse": pownArray[raida_id] = 'n'; break;
            }//end switch
            cc.pown = new string(pownArray);
            return true;
        }//end set past status

        public void  setAOID_NoReplyAN(int RAIDA, string Old_AN)//If the RAIDA had a no reply, save the PAN here incase we need to use it again
        {
            //Sample 1=4b07dc3f076e4605a9eb4570c859e2c9
            string backup_AN = RAIDA + "=" + Old_AN;
            cc.aoid.Add(backup_AN);
        }//end set aoid no reply pan

        public string getAOID_NoReplyAN(int RAIDA)//Returns an AN of CloudCoin that got a reply 
        {
            string returnAn = "none_found";
            foreach (string an in cc.aoid)
            {
                //Sample 1=4b07dc3f076e4605a9eb4570c859e2c9
                string[] aoidParts = an.Split('=');
                if( aoidParts[0] == RAIDA.ToString()){
                    return aoidParts[1];
                }//end if
            }
            
            return returnAn;
        }//end set aoid no reply pan

        public string getFolder()
        {
            string returnString = "";
            switch (folder)
            {
                case Folder.Bank: returnString = "Bank"; break;
                case Folder.Counterfeit: returnString = "Counterfeit"; break;
                case Folder.Fracked: returnString = "Fracked"; break;
                case Folder.Suspect: returnString = "Suspect"; break;
                case Folder.Detected: returnString = "Detected"; break;
                case Folder.Dangerous: returnString = "Dangerous"; break;
                case Folder.Trash: returnString = "Trash"; break;
                case Folder.Lost: returnString = "Lost"; break;
            }//end switch
            return returnString;
        }//end getPastStatus

        public bool setFolder(string folderName)
        {
            bool setGood = false;
            switch (folderName.ToLower())
            {
                case "bank": folder = Folder.Bank; break;
                case "counterfeit": folder = Folder.Counterfeit; break;
                case "fracked": folder = Folder.Fracked; break;
                case "suspect": folder = Folder.Suspect; break;
                case "trash": folder = Folder.Trash; break;
                case "danger": folder = Folder.Fracked; break;
                case "lost": folder = Folder.Suspect; break;
                case "detected": folder = Folder.Trash; break;
            }//end switch
            return setGood;
        }//end set past status

        public int getDenomination()
        {
            int nom = 0;
            if ((cc.sn < 1))
            {
                nom = 0;
            }
            else if ((cc.sn < 2097153))
            {
                nom = 1;
            }
            else if ((cc.sn < 4194305))
            {
                nom = 5;
            }
            else if ((cc.sn < 6291457))
            {
                nom = 25;
            }
            else if ((cc.sn < 14680065))
            {
                nom = 100;
            }
            else if ((cc.sn < 16777217))
            {
                nom = 250;
            }
            else
            {
                nom = '0';
            }

            return nom;
        }//end get denomination

        public void calculateHP()
        {
            hp = 25;
            char[] pownArray = cc.pown.ToCharArray();
            for (int i = 0; (i < 25); i++)
            {
                if (pownArray[i] == 'f')
                {
                    this.hp--;
                }
            }
        }//end calculate hp

        public void calcExpirationDate()
        {
            DateTime expirationDate = DateTime.Today.AddYears(YEARSTILEXPIRE);
            cc.ed = (expirationDate.Month + "-" + expirationDate.Year);
            //  Console.WriteLine("ed = " + cc.ed);
            DateTime zeroDate = new DateTime(2016, 08, 13);
            // DateTime zeroDate = DateTime.Parse("8/13/2016 8:33:21 AM");
            int monthsAfterZero = (int)(expirationDate.Subtract(zeroDate).Days / (365.25 / 12));
            //Turn positive and up to down to floor
            // Console.WriteLine("Months after zero = " + monthsAfterZero);
            this.edHex = monthsAfterZero.ToString("X2");
        }// end calc exp date

        public String generatePan()
        {
            byte[] cryptoRandomBuffer = new byte[16];
            NetFxCrypto.RandomNumberGenerator.GetBytes(cryptoRandomBuffer);

            Guid pan = new Guid(cryptoRandomBuffer);
            String rawpan = pan.ToString("N");
            String fullPan = "";
            switch (rawpan.Length)//Make sure the pan is 32 characters long. The odds of this happening are slim but it will happen.
            {
                case 27: fullPan = ("00000" + rawpan); break;
                case 28: fullPan = ("0000" + rawpan); break;
                case 29: fullPan = ("000" + rawpan); break;
                case 30: fullPan = ("00" + rawpan); break;
                case 31: fullPan = ("0" + rawpan); break;
                case 32: fullPan = rawpan; break;
                case 33: fullPan = rawpan.Substring(0, rawpan.Length - 1); break;//trim one off end
                case 34: fullPan = rawpan.Substring(0, rawpan.Length - 2); break;//trim one off end
            }

            return fullPan;
        }

        public String[] grade()
        {
            int passed = 0;
            int failed = 0;
            int other = 0;
            String passedDesc = "";
            String failedDesc = "";
            String otherDesc = "";
            char[] pownArray = cc.pown.ToCharArray();

            for (int i = 0; (i < 25); i++)
            {
                if (pownArray[i] == 'p')
                {
                    passed++;
                }
                else if (pownArray[i] == 'f')
                {
                    failed++;
                }
                else
                {
                    other++;
                }// end if pass, fail or unknown
            }

            // for each status
            // Calculate passed
            if (passed == 25)
            {
                passedDesc = "100% Passed!";
            }
            else if (passed > 17)
            {
                passedDesc = "Super Majority";
            }
            else if (passed > 13)
            {
                passedDesc = "Majority";
            }
            else if (passed == 0)
            {
                passedDesc = "None";
            }
            else if (passed < 5)
            {
                passedDesc = "Super Minority";
            }
            else
            {
                passedDesc = "Minority";
            }

            // Calculate failed
            if (failed == 25)
            {
                failedDesc = "100% Failed!";
            }
            else if (failed > 17)
            {
                failedDesc = "Super Majority";
            }
            else if (failed > 13)
            {
                failedDesc = "Majority";
            }
            else if (failed == 0)
            {
                failedDesc = "None";
            }
            else if (failed < 5)
            {
                failedDesc = "Super Minority";
            }
            else
            {
                failedDesc = "Minority";
            }

            // Calcualte Other RAIDA Servers did not help. 
            switch (other)
            {
                case 0:
                    otherDesc = "100% of RAIDA responded";
                    break;
                case 1:
                case 2:
                    otherDesc = "Two or less RAIDA errors";
                    break;
                case 3:
                case 4:
                    otherDesc = "Four or less RAIDA errors";
                    break;
                case 5:
                case 6:
                    otherDesc = "Six or less RAIDA errors";
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    otherDesc = "Between 7 and 12 RAIDA errors";
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    otherDesc = "RAIDA total failure";
                    break;
                default:
                    otherDesc = "FAILED TO EVALUATE RAIDA HEALTH";
                    break;
            }
            // end RAIDA other errors and unknowns
            // Coin will go to bank, counterfeit or fracked
            if (other > 12)
            {
                // not enough RAIDA to have a quorum
                folder = Folder.Suspect;
            }
            else if (failed > passed)
            {
                // failed out numbers passed with a quorum: Counterfeit
                folder = Folder.Counterfeit;
            }
            else if (failed > 0)
            {
                // The quorum majority said the coin passed but some disagreed: fracked. 
                folder = Folder.Fracked;
            }
            else
            {
                // No fails, all passes: bank
                folder = Folder.Bank;
            }

            gradeStatus[0] = passedDesc;
            gradeStatus[1] = failedDesc;
            gradeStatus[2] = otherDesc;
            return this.gradeStatus;
        }// end gradeStatus

        public bool isGradablePass()
        {
            //The coin is considered ungradable if it does not get more than 19 RAIDA available
            bool returnTruth = false;
            if (charCount(cc.pown, 'f') + charCount(cc.pown, 'p') > 16 && isFixable() && !isDangerous())
            {
                returnTruth = true;
                Console.Out.WriteLine("isGradable");
            }
            else {
                Console.Out.WriteLine("Not isGradable");
            }
            return returnTruth;
        }//end is gradable pass

        public bool isPerfect()
        {
            //The coin is considered perfect if it has all passes
            bool returnTruth = false;
            if (cc.pown == "ppppppppppppppppppppppppp")
            {
                returnTruth = true;
                Console.Out.WriteLine("isPerfect");
            }
            else {
                Console.Out.WriteLine("Not isPerfect");
            }
            return returnTruth;
        }//end is perfect

        public bool isCounterfeit()
        {
            //The coin is considered counterfeit if it has so many fails it cannot be fixed
            bool returnTruth = false;
            if ( (charCount(cc.pown, 'p')  < 6 && (charCount(cc.pown, 'f') > 13))) {
                returnTruth = true;
                Console.Out.WriteLine("isCounterfeit");
            }
            else
            {
                Console.Out.WriteLine("Not isCounterfeit");
            }
            return returnTruth;
        }//end is counterfeit

        public bool isFracked()
        {
            //The coin is considered fracked if it has any fails
            bool returnTruth = false;
            if (charCount(cc.pown, 'f') > 0 || charCount(cc.pown, 'n') > 0) {
                returnTruth = true;
                Console.Out.WriteLine("isFracked");
            }
            else
            {
                Console.Out.WriteLine("Not isFracked");
            }
            return returnTruth;
        }//end is fracked

        public bool noResponses()
        {
            //Does the coin have no-responses from the RIDA. This means the RAIDA may be using its PAN or AN
            //These must be fixed in a special way using both.  
            bool returnTruth = false;
            if ( charCount(cc.pown, 'n') > 0)
            {
                returnTruth = true;
                Console.Out.WriteLine("noResponses");
            }
            else
            {
                Console.Out.WriteLine("Not noResponses");
            }
            return returnTruth;
        }//end is fracked

        public bool isDangerous()
        {
            //The coin is considered a threat if it has any of the patersns that would allow the last user to take control.
            //There are four of these patterns: One for each corner. 
            bool threat = false;
          //  Console.Out.WriteLine( cc.sn + " char count f =" + charCount(cc.pown, 'f'));
            if ( (charCount(cc.pown, 'f') + charCount(cc.pown, 'n')) > 5)
            {
                string doublePown = cc.pown + cc.pown;//double it so we see patters that happen on the ends.
                Match UP_LEFT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]fp", RegexOptions.IgnoreCase);//String UP_LEFT = "ff***f";
                Match UP_RIGHT = Regex.Match(doublePown, @"ff[a-z][a-z][a-z]pf", RegexOptions.IgnoreCase);//String UP_RIGHT = "ff***pf";
                Match DOWN_LEFT = Regex.Match(doublePown, @"fp[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);//String DOWN_LEFT = "fp***ff";
                Match DOWN_RIGHT = Regex.Match(doublePown, @"pf[a-z][a-z][a-z]ff", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "pf***ff";
                //Check if it has a weakness
               // if (UP_LEFT.Success) { Console.Out.WriteLine("up left match"); }//end
                //if (UP_RIGHT.Success) { Console.Out.WriteLine("up right match"); }//end
               // if (DOWN_LEFT.Success) { Console.Out.WriteLine("down left match"); }//end
               // if (DOWN_RIGHT.Success) { Console.Out.WriteLine("down right match"); }//end

                if (UP_LEFT.Success || UP_RIGHT.Success || DOWN_LEFT.Success || DOWN_RIGHT.Success)
                {
                    threat = true;
                    Console.Out.WriteLine("isDangerous");
                }//end if coin contains threats.
            }
            else
            {
                Console.Out.WriteLine("Not isDangerous");
            }
            return threat;
        }//end is threat

        public bool isFixable()
        {
            //The coin is considered fixable if it has any of the patersns that would allow the new owner to fix fracked.
            //There are four of these patterns: One for each corner. 
            bool canFix = false;
           // Console.Out.WriteLine(cc.sn + " char count p =" + charCount(cc.pown, 'p'));
            if (charCount(cc.pown, 'p') > 5)
            {
                string doublePown = cc.pown + cc.pown;//double it so we see patters that happen on the ends.
                Match UP_LEFT = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pf", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pf";
                Match UP_RIGHT = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]fp", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***fp";
                Match DOWN_LEFT = Regex.Match(doublePown, @"pf[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pf***pp";
                Match DOWN_RIGHT = Regex.Match(doublePown, @"fp[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "fp***pp";

                Match UP_LEFT_n = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pn", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pn";
                Match UP_RIGHT_n = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]np", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***np";
                Match DOWN_LEFT_n = Regex.Match(doublePown, @"pn[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pn***pp";
                Match DOWN_RIGHT_n = Regex.Match(doublePown, @"np[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "np***pp";

                Match UP_LEFT_e = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pe", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pe";
                Match UP_RIGHT_e = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]ep", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***ep";
                Match DOWN_LEFT_e = Regex.Match(doublePown, @"pe[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pe***pp";
                Match DOWN_RIGHT_e = Regex.Match(doublePown, @"ep[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "ep***pp";

                Match UP_LEFT_u = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]pu", RegexOptions.IgnoreCase);//String UP_LEFT = "pp***pu";
                Match UP_RIGHT_u = Regex.Match(doublePown, @"pp[a-z][a-z][a-z]up", RegexOptions.IgnoreCase);//String UP_RIGHT = "pp***up";
                Match DOWN_LEFT_u = Regex.Match(doublePown, @"pu[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_LEFT = "pu***pp";
                Match DOWN_RIGHT_u = Regex.Match(doublePown, @"up[a-z][a-z][a-z]pp", RegexOptions.IgnoreCase);//String DOWN_RIGHT = "up***pp";

                if (UP_LEFT.Success || UP_RIGHT.Success || DOWN_LEFT.Success || DOWN_RIGHT.Success || UP_LEFT_n.Success || UP_RIGHT_n.Success || DOWN_LEFT_n.Success || DOWN_RIGHT_n.Success || UP_LEFT_e.Success || UP_RIGHT_e.Success || DOWN_LEFT_e.Success || DOWN_RIGHT_e.Success || UP_LEFT_u.Success || UP_RIGHT_u.Success || DOWN_LEFT_u.Success || DOWN_RIGHT_u.Success)
                {
                    canFix = true;
                    Console.Out.WriteLine("isFixable");
                }
                else
                {
                    canFix = false;
                    Console.Out.WriteLine("Not isFixable");
                }


                // if (UP_LEFT.Success) { Console.Out.WriteLine("canFix up left match"); }//end
                // if (UP_RIGHT.Success) { Console.Out.WriteLine("canFix up right match"); }//end
                // // if (DOWN_LEFT.Success) { Console.Out.WriteLine("canFix down left match"); }//end
                // if (DOWN_RIGHT.Success) { Console.Out.WriteLine("canFix down right match"); }//end
                //if (UP_LEFT_n.Success) { Console.Out.WriteLine("canFix_n up left match"); }//end
                // if (UP_RIGHT_n.Success) { Console.Out.WriteLine("canFix_n up right match"); }//end
                // if (DOWN_LEFT_n.Success) { Console.Out.WriteLine("canFix_n down left match"); }//end
                // if (DOWN_RIGHT_n.Success) { Console.Out.WriteLine("canFix_n down right match"); }//end
            }//end if more than five passed
            else
            {
                canFix = false;
                Console.Out.WriteLine("Not isFixable");
            }
            return canFix;
        }//end is fixable

        public void recordPown()
        {
            //records the last pown so we can see if there are improvments
            pastPown = cc.pown;
        }//end record pown

        public void sortToFolder()
        {
            //figures out which folder to put it in. 
            if (isPerfect())
            {
                folder = Folder.Bank;
                return;
            }//if is perfect

            if (isCounterfeit())
            {
                folder = Folder.Counterfeit;
                return;
            }//if is counterfeit

            if (!isGradablePass())
            {
                if ( noResponses() ) {
                    folder = Folder.Lost;
                    return;
                }//end no responses
                folder = Folder.Suspect;
                return;
            }//if is gradable

            if (!isFracked())
            {
                folder = Folder.Bank;
                return;
            }//if is has no failes but some undetected but is gradable

            //--------------------------------------
            /*Now look  at fracked coins*/

            if (isDangerous())//Previous owner could try to take it back. 
            {

                if (!isFixable())
                {
                    folder = Folder.Counterfeit;
                    Console.Out.WriteLine("!isFixable");
                    return;
                }//end if not fixable
            }
            else
            {
                if ( isFixable() )
                {
                    folder = Folder.Fracked;
                    return;
                }//end if not fixable 
            }//end if is dangerous

            recordPown();
            folder = Folder.Dangerous;//If you get down here, the coin is dangerous and needs to be defracked then detected again.
        }//end sort folder

        public void sortFoldersAfterFixingDangerous()
        {
            if (!isFracked())
            {
                folder = Folder.Suspect;
                Console.Out.WriteLine("sort after !isFixable");
                return;
            }

            if (!isFixing())
            {
                folder = Folder.Suspect;
                Console.Out.WriteLine("sort after !isFixing");
                return;
            }
            //else just keep in dangerous
            Console.Out.WriteLine("nothing sorted after");
        }//end sort after fixing dangerous

        public bool isFixing()
        {
            //The coin is considered to be fixing if the current pown is better than the past pown
            bool returnTruth = false;
            if (charCount(cc.pown, 'p') > charCount(pastPown, 'p')) { returnTruth = true; }
            return returnTruth;
        }//end is fracked

        public int charCount(string pown, char character)
        {
            return pown.Count(x => x == character);
        }

        public void setAnsToPans()
        {
            for (int i = 0; (i < 25); i++)
            {
                this.pans[i] = cc.an[i];
            }// end for 25 ans
        }// end setAnsToPans

        public void setAnsToPansIfPassed(bool partial = false)
        {
            // now set all ans that passed to the new pans
            char[] pownArray = cc.pown.ToCharArray();

            for (int i = 0; (i < 25); i++)
            {
                if (pownArray[i] == 'p')//1 means pass
                {
                    cc.an[i] = pans[i];
                }
                if (pownArray[i] == 'n')//n means there was no reply from the detection request. The RAIDA is probably there because it passed echo (otherwise it would not have been tried. But we don't know. 
                {
                    //So we must now save the AN and the PAN so we can try to fix it later should it need to. 
                    //We assume that the RAIDA got the message but we did not hear back from it. 
                    cc.an[i] = pans[i];
                    setAOID_NoReplyAN(i, cc.an[i]);//This will put a note and the AN in the AOID so we know what it was.  
                }
                else if (pownArray[i] == 'u')//We did not try to detect because the RAIDA was not echoing. So we cannot update AN. 
                {
                    //Just keep the An the same by doing nothing.
                }
                else//Else error or fail. 
                {
                    // If it is an error we want to keep the an.
                    //If it is a fail it does not matter.  
                }
            }// for each guid in coin
        }// end set ans to pans if passed

        public void consoleReport()
        {
            // Used only for console apps
            //  System.out.println("Finished detecting coin index " + j);
            // PRINT OUT ALL COIN'S RAIDA STATUS AND SET AN TO NEW PAN
            char[] pownArray = cc.pown.ToCharArray();
            sortToFolder();
            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Out.WriteLine("                                                        ");
            Console.Out.WriteLine("   Authenticity Report SN #" + string.Format("{0,8}", cc.sn) + ", Denomination: " + string.Format("{0,3}", this.getDenomination()) + "  ");
            
            CoreLogger.Log("   Authenticity Report SN #" + string.Format("{0,8}", cc.sn) + ", Denomination: " + string.Format("{0,3}", this.getDenomination()) + "  ");
            Console.Out.WriteLine("                                                        ");
            Console.Out.Write("    "); a(pownArray[0]); Console.Out.Write("       "); a(pownArray[1]); Console.Out.Write("       "); a(pownArray[2]); Console.Out.Write("       "); a(pownArray[3]); Console.Out.Write("       "); a(pownArray[4]); Console.Out.WriteLine("    ");
            Console.Out.WriteLine("                                                        ");
            Console.Out.Write("    "); a(pownArray[5]); Console.Out.Write("       "); a(pownArray[6]); Console.Out.Write("       "); a(pownArray[7]); Console.Out.Write("       "); a(pownArray[8]); Console.Out.Write("       "); a(pownArray[9]); Console.Out.WriteLine("    ");
            Console.Out.WriteLine("                                                        ");
            Console.Out.Write("    "); a(pownArray[10]); Console.Out.Write("       "); a(pownArray[11]); Console.Out.Write("       "); a(pownArray[12]); Console.Out.Write("       "); a(pownArray[13]); Console.Out.Write("       "); a(pownArray[14]); Console.Out.WriteLine("    ");
            Console.Out.WriteLine("                                                        ");
            Console.Out.Write("    "); a(pownArray[15]); Console.Out.Write("       "); a(pownArray[16]); Console.Out.Write("       "); a(pownArray[17]); Console.Out.Write("       "); a(pownArray[18]); Console.Out.Write("       "); a(pownArray[19]); Console.Out.WriteLine("    ");
            Console.Out.WriteLine("                                                ");
            Console.Out.Write("    "); a(pownArray[20]); Console.Out.Write("       "); a(pownArray[21]); Console.Out.Write("       "); a(pownArray[22]); Console.Out.Write("       "); a(pownArray[23]); Console.Out.Write("       "); a(pownArray[24]); Console.Out.WriteLine("    ");
            Console.Out.WriteLine("   Status: " + getFolder());
            Console.Out.WriteLine("                                                        ");
            Console.Out.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;

            // check if failed
            //  string fmt = "00";
            // string fi = i.ToString(fmt); // Pad numbers with two digits
            //    Console.Out.WriteLine("RAIDA" + i + " " + pastStatus[i] + " | ");
            // Console.Out.WriteLine("AN " + i + ans[i]);
            // Console.Out.WriteLine("PAN " + i + pans[i]);
            //  }

            // End for each cloud coin GUID statu
            //  Console.Out.WriteLine("ed " + ed);
            //  Console.Out.WriteLine("edHex " + edHex);
            //  Console.Out.WriteLine("edhp " + hp);
            // Console.Out.WriteLine("fileName " + fileName);
            // Console.Out.WriteLine("YEARSTILEXPIRE " + YEARSTILEXPIRE);
            //   Console.Out.WriteLine("extension " + extension);


        }//Console Report

        public void a(Char status)
        {
            if (status == 'p')
            {
                Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Green; Console.Out.Write("Pass"); Console.ForegroundColor = ConsoleColor.White;
            }
            else if (status == 'f')
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Out.Write("Fail"); Console.ForegroundColor = ConsoleColor.White;
            }
            else if (status == 'n')
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("NoRe"); Console.ForegroundColor = ConsoleColor.White;
            }
            else if (status == 'e')
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("Erro"); Console.ForegroundColor = ConsoleColor.White;
            }
            else if (status == 'u')
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("Unde"); Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("Skip"); Console.ForegroundColor = ConsoleColor.White;
            }
        }//end a Report helper
    }
}
