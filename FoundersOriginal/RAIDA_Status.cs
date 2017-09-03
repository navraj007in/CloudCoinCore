
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
    }//end RAIDA_Status
}//End name space
