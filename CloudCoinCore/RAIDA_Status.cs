using System;
using System.Linq;
namespace Founders
{
    public static class RAIDA_Status
    {

        public static bool[] failsEcho = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        public static int[] echoTime = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static bool[] failsDetect = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        public static bool[] failsFix = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        public static bool[] hasTicket = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        public enum TicketHistory { Untried, Failed, Success };

        public static TicketHistory[] ticketHistory = { TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried, TicketHistory.Untried };

        public static string[] tickets = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };

        /*Used every time the user will try to unfrack another RAIDA on the same coin.*/
        public static void resetTickets()
        {
            for (int i = 0; i < 25; i++)
            {
                hasTicket[i] = false;
                ticketHistory[i] = TicketHistory.Untried;
                tickets[i] = "";
            }//end for each RAIDA
        }//end reset Tickers


        public static int getLowest21()//21 servers is good enough to pown it
        {
            int fifthSlowest = (from number in echoTime
                                orderby number descending
                                select number).Distinct().Skip(5).First();
            return fifthSlowest;
        }

        public static void resetEcho()
        {
            for (int i = 0; i < 25; i++)
            {
                failsEcho[i] = false;
            }//end for each RAIDA
        }//end reset Tickers

        /*Used every time the user will try to unfrack another coin from the start.*/
        public static void newCoin()
        {
            for (int i = 0; i < 25; i++)
            {
                hasTicket[i] = false;
                ticketHistory[i] = TicketHistory.Untried;
                tickets[i] = "";
                failsDetect[i] = false;
            }//end for each RAIDA
        }//end new coin 

   /*     public static void print_echo()
        {
            for (int i = 0; i < 25; i++)
            {
                

                Console.Out.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Out.WriteLine("                                                        ");
                Console.Out.WriteLine("  Time to Echo Servers in Milliseconds");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(failsEcho[0]); Console.Out.Write("       "); a(failsEcho[1]); Console.Out.Write("       "); a(failsEcho[2]); Console.Out.Write("       "); a(failsEcho[3]); Console.Out.Write("       "); a(failsEcho[4]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(failsEcho[5]); Console.Out.Write("       "); a(failsEcho[6]); Console.Out.Write("       "); a(failsEcho[7]); Console.Out.Write("       "); a(failsEcho[8]); Console.Out.Write("       "); a(failsEcho[9]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(failsEcho[10]); Console.Out.Write("       "); a(failsEcho[11]); Console.Out.Write("       "); a(failsEcho[12]); Console.Out.Write("       "); a(failsEcho[13]); Console.Out.Write("       "); a(failsEcho[14]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(failsEcho[15]); Console.Out.Write("       "); a(failsEcho[16]); Console.Out.Write("       "); a(failsEcho[17]); Console.Out.Write("       "); a(failsEcho[18]); Console.Out.Write("       "); a(failsEcho[19]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                ");
                Console.Out.Write("    "); a(failsEcho[20]); Console.Out.Write("       "); a(failsEcho[21]); Console.Out.Write("       "); a(failsEcho[22]); Console.Out.Write("       "); a(failsEcho[23]); Console.Out.Write("       "); a(failsEcho[24]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
            }//end for each RAIDA
        }//end new coin 
        */

        public static void showMs()
        {
                Console.Out.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Out.WriteLine("                                                        ");
                Console.Out.WriteLine("  Time to Echo RAIDA Clusters in Milliseconds");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(echoTime[0]); Console.Out.Write("       "); a(echoTime[1]); Console.Out.Write("       "); a(echoTime[2]); Console.Out.Write("       "); a(echoTime[3]); Console.Out.Write("       "); a(echoTime[4]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(echoTime[5]); Console.Out.Write("       "); a(echoTime[6]); Console.Out.Write("       "); a(echoTime[7]); Console.Out.Write("       "); a(echoTime[8]); Console.Out.Write("       "); a(echoTime[9]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(echoTime[10]); Console.Out.Write("       "); a(echoTime[11]); Console.Out.Write("       "); a(echoTime[12]); Console.Out.Write("       "); a(echoTime[13]); Console.Out.Write("       "); a(echoTime[14]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.Write("    "); a(echoTime[15]); Console.Out.Write("       "); a(echoTime[16]); Console.Out.Write("       "); a(echoTime[17]); Console.Out.Write("       "); a(echoTime[18]); Console.Out.Write("       "); a(echoTime[19]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                ");
                Console.Out.Write("    "); a(echoTime[20]); Console.Out.Write("       "); a(echoTime[21]); Console.Out.Write("       "); a(echoTime[22]); Console.Out.Write("       "); a(echoTime[23]); Console.Out.Write("       "); a(echoTime[24]); Console.Out.WriteLine("    ");
                Console.Out.WriteLine("                                                        ");
                Console.Out.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;
        }//end new coin 



        public static void a(int ms)
        {
            int goodTime = getLowest21();
            if (ms == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Out.Write(ms); Console.ForegroundColor = ConsoleColor.White;

            }
            else if (ms < goodTime)
            {
                Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Green; Console.Out.Write(ms); Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write(ms); Console.ForegroundColor = ConsoleColor.White;
            }
        }//end a Report helper




        public static void a(bool status)
        {
            if (status )
            {
                Console.ForegroundColor = Console.ForegroundColor = ConsoleColor.Green; Console.Out.Write("Ready"); Console.ForegroundColor = ConsoleColor.White;
            }
            else 
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.Out.Write("Not"); Console.ForegroundColor = ConsoleColor.White;
            }
        }//end a Report helper


        public static void b(char status)
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
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("Slow"); Console.ForegroundColor = ConsoleColor.White;
            }
            else if (status == 'e')
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("Flaw"); Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Out.Write("Skip"); Console.ForegroundColor = ConsoleColor.White;
            }
        }//end a Report helper


    }//end RAIDA_Status
}//End name space
