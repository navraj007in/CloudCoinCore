
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Founders
{
    public class RAIDA
    {
        /* INSTANCE VARIABLE */
        public DetectionAgent[] agent;
        //public CloudCoin returnCoin;
        public Response[] responseArray = new Response[25];
        private int[] working_triad = { 0, 1, 2 };//place holder
        public bool[] raidaIsDetecting = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
        public string[] lastDetectStatus = { "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected" };
        public string[] echoStatus = { "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply" };

        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };


        /* CONSTRUCTOR */
        public RAIDA(int milliSecondsToTimeOut)
        { //  initialise instance variables
            this.agent = new DetectionAgent[25];
            
            for (int i = 0; (i < 25); i++)
            {
                this.agent[i] = new DetectionAgent(i, milliSecondsToTimeOut);
            } // end for each Raida
        }//End Constructor


        public async Task echoOne(int raida_id)
        {
            DetectionAgent da = new DetectionAgent(raida_id, 2000);
            responseArray[raida_id] = await da.echo(raida_id);
            
            
            Console.Write(".");
        }//end echo 




        public Response[] echoAll(int milliSecondsToTimeOut)
        {


            Response[] results = new Response[25];
            var t00 =   echoOne(00);
            var t01 = echoOne(01);
            var t02 = echoOne(02);
            var t03 = echoOne(03);
            var t04 =  echoOne(04);
            var t05 =  echoOne(05);
            var t06 =  echoOne(06);
            var t07 =  echoOne(07);
            var t08 =  echoOne(08);
            var t09 =  echoOne(09);
            var t10 =  echoOne(10);
            var t11 =  echoOne(11);
            var t12 =  echoOne(12);
            var t13 = echoOne(13);
            var t14 =  echoOne(14);
            var t15 =  echoOne(15);
            var t16 =  echoOne(16);
            var t17 = echoOne(17);
            var t18 =  echoOne(18);
            var t19 =  echoOne(19);
            var t20 =  echoOne(20);
            var t21 =  echoOne(21);
            var t22 =  echoOne(22);
            var t23 =  echoOne(23);
            var t24 =  echoOne(24);

            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            for(int i=0; i<25; i++)
                if(responseArray[i] != null)
                CoreLogger.Log("echo:" + i + " " + responseArray[i].fullResponse);

            
            return results;
        }//end echo



        public async Task detectOne(int raida_id, int nn, int sn, String an, String pan, int d)
        {
            DetectionAgent da = new DetectionAgent(raida_id, 5000);
            responseArray[raida_id] = await da.detect(nn, sn, an, pan, d);
            
            
        }//end detectOne



        public CoinUtils detectCoin( CoinUtils cu, int milliSecondsToTimeOut )
        {
            cu.generatePan();

            var t00 =  detectOne( 00, cu.cc.nn, cu.cc.sn, cu.cc.an[00], cu.pans[00], cu.getDenomination());
            var t01 =  detectOne( 01, cu.cc.nn, cu.cc.sn, cu.cc.an[01], cu.pans[01], cu.getDenomination());
            var t02 = detectOne( 02, cu.cc.nn, cu.cc.sn, cu.cc.an[02], cu.pans[02], cu.getDenomination());
            var t03 =  detectOne( 03, cu.cc.nn, cu.cc.sn, cu.cc.an[03], cu.pans[03], cu.getDenomination());
            var t04 =  detectOne( 04, cu.cc.nn, cu.cc.sn, cu.cc.an[04], cu.pans[04], cu.getDenomination());
            var t05 =  detectOne( 05, cu.cc.nn, cu.cc.sn, cu.cc.an[05], cu.pans[05], cu.getDenomination());
            var t06 =  detectOne( 06, cu.cc.nn, cu.cc.sn, cu.cc.an[06], cu.pans[06], cu.getDenomination());
            var t07 =  detectOne( 07, cu.cc.nn, cu.cc.sn, cu.cc.an[07], cu.pans[07], cu.getDenomination());
            var t08 =  detectOne( 08, cu.cc.nn, cu.cc.sn, cu.cc.an[08], cu.pans[08], cu.getDenomination());
            var t09 =  detectOne( 09, cu.cc.nn, cu.cc.sn, cu.cc.an[09], cu.pans[09], cu.getDenomination());
            var t10 =  detectOne( 10, cu.cc.nn, cu.cc.sn, cu.cc.an[10], cu.pans[10], cu.getDenomination());
            var t11 =  detectOne( 11, cu.cc.nn, cu.cc.sn, cu.cc.an[11], cu.pans[11], cu.getDenomination());
            var t12 =  detectOne( 12, cu.cc.nn, cu.cc.sn, cu.cc.an[12], cu.pans[12], cu.getDenomination());
            var t13 =  detectOne( 13, cu.cc.nn, cu.cc.sn, cu.cc.an[13], cu.pans[13], cu.getDenomination());
            var t14 = detectOne( 14, cu.cc.nn, cu.cc.sn, cu.cc.an[14], cu.pans[14], cu.getDenomination());
            var t15 =  detectOne( 15, cu.cc.nn, cu.cc.sn, cu.cc.an[15], cu.pans[15], cu.getDenomination());
            var t16 =  detectOne( 16, cu.cc.nn, cu.cc.sn, cu.cc.an[16], cu.pans[16], cu.getDenomination());
            var t17 =  detectOne( 17, cu.cc.nn, cu.cc.sn, cu.cc.an[17], cu.pans[17], cu.getDenomination());
            var t18 =  detectOne( 18, cu.cc.nn, cu.cc.sn, cu.cc.an[18], cu.pans[18], cu.getDenomination());
            var t19 =  detectOne( 19, cu.cc.nn, cu.cc.sn, cu.cc.an[19], cu.pans[19], cu.getDenomination());
            var t20 =  detectOne( 20, cu.cc.nn, cu.cc.sn, cu.cc.an[20], cu.pans[20], cu.getDenomination());
            var t21 =  detectOne( 21, cu.cc.nn, cu.cc.sn, cu.cc.an[21], cu.pans[21], cu.getDenomination());
            var t22 =  detectOne( 22, cu.cc.nn, cu.cc.sn, cu.cc.an[22], cu.pans[22], cu.getDenomination());
            var t23 = detectOne( 23, cu.cc.nn, cu.cc.sn, cu.cc.an[23], cu.pans[23], cu.getDenomination());
            var t24 =  detectOne( 24, cu.cc.nn, cu.cc.sn, cu.cc.an[24], cu.pans[24], cu.getDenomination());


            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            //Get data from the detection agents

            for (int i = 0; i < 25; i++)
            {
                if (responseArray[i] != null)
                {
                    cu.setPastStatus(responseArray[i].outcome, i);
                    CoreLogger.Log(cu.cc.sn + " detect:" + i + " " + responseArray[i].fullResponse);
                }
                else
                {
                    cu.setPastStatus("undetected", i);
                };// should be pass, fail, error or undetected. 
            }//end for each detection agent

            cu.setAnsToPansIfPassed();
            cu.calculateHP();
           // cu.gradeCoin(); // sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            cu.calcExpirationDate();
            cu.grade();
         
            return cu;
        }//end detect coin

        public CoinUtils partialDetectCoin(CoinUtils cu, int milliSecondsToTimeOut)
        {
            cu.generatePan();
            int[] echoes = (int[]) RAIDA_Status.echoTime.Clone();
            int[] raidas = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            Array.Sort(echoes, raidas);
            Console.WriteLine("fastest raida: " + raidas[0] + "," + raidas[1] + "," + raidas[2] + "," + raidas[3] + "," + raidas[4] + "," + raidas[5] + "," + raidas[6] + "," + raidas[7] + "," + raidas[8] + "," + raidas[9] + "," + raidas[10] + "," + raidas[11] + "," + raidas[12] + "," + raidas[13] + "," + raidas[14] + "," + raidas[15]);
            var t00 =  detectOne(raidas[00], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[00]], cu.pans[raidas[00]], cu.getDenomination());
            var t01 =  detectOne(raidas[01], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[01]], cu.pans[raidas[01]], cu.getDenomination());
            var t02 =  detectOne(raidas[02], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[02]], cu.pans[raidas[02]], cu.getDenomination());
            var t03 =  detectOne(raidas[03], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[03]], cu.pans[raidas[03]], cu.getDenomination());
            var t04 =  detectOne(raidas[04], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[04]], cu.pans[raidas[04]], cu.getDenomination());
            var t05 =  detectOne(raidas[05], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[05]], cu.pans[raidas[05]], cu.getDenomination());
            var t06 =  detectOne(raidas[06], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[06]], cu.pans[raidas[06]], cu.getDenomination());
            var t07 =  detectOne(raidas[07], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[07]], cu.pans[raidas[07]], cu.getDenomination());
            var t08 =  detectOne(raidas[08], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[08]], cu.pans[raidas[08]], cu.getDenomination());
            var t09 =  detectOne(raidas[09], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[09]], cu.pans[raidas[09]], cu.getDenomination());
            var t10 =  detectOne(raidas[10], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[10]], cu.pans[raidas[10]], cu.getDenomination());
            var t11 =  detectOne(raidas[11], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[11]], cu.pans[raidas[11]], cu.getDenomination());
            var t12 =  detectOne(raidas[12], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[12]], cu.pans[raidas[12]], cu.getDenomination());
            var t13 =  detectOne(raidas[13], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[13]], cu.pans[raidas[13]], cu.getDenomination());
            var t14 =  detectOne(raidas[14], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[14]], cu.pans[raidas[14]], cu.getDenomination());
            var t15 =  detectOne(raidas[15], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[15]], cu.pans[raidas[15]], cu.getDenomination());
            //var t16 = Task.Run(() => detectOne(raidas[16], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[16]], cu.pans[raidas[16]], cu.getDenomination()));
            


            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            //Get data from the detection agents

            //nt k = 0;
            //for(int j =0; j<16;j++)
            //{
            //    if(responseArray[raidas[j]] != null && responseArray[raidas[j]].outcome == "error" && k < 9)
            //    {
            //        detectOne(raidas[16+k], cu.cc.nn, cu.cc.sn, cu.cc.an[raidas[16+k]], cu.pans[raidas[16+k]], cu.getDenomination());
             //       k++;
             //   }
            //}

            for (int i = 0; i < 25; i++)
            {
                if (responseArray[i] != null)
                {
                    cu.setPastStatus(responseArray[i].outcome, i);
                    CoreLogger.Log(cu.cc.sn + " detect:" + i + " " + responseArray[i].fullResponse);
                }
                else
                {
                    cu.setPastStatus("undetected", i);
                };// should be pass, fail, error or undetected. 
            }//end for each detection agent

            cu.setAnsToPansIfPassed(true);
            cu.calculateHP();
            // cu.gradeCoin(); // sets the grade and figures out what the file extension should be (bank, fracked, counterfeit, lost
            cu.calcExpirationDate();
            cu.grade();

            return cu;
        }//end detect coin


        public async Task get_Ticket(int i, int raidaID, int nn, int sn, String an, int d)
        {
            DetectionAgent da = new DetectionAgent(raidaID, 5000);
            responseArray[raidaID] = await da.get_ticket(nn, sn, an, d);
            
           
        }//end get ticket


        public void get_Tickets(int[] triad, String[] ans, int nn, int sn, int denomination, int milliSecondsToTimeOut)
        {
            //Console.WriteLine("Get Tickets called. ");
            var t00 =  get_Ticket(0, triad[00], nn, sn, ans[00], denomination);
            var t01 =  get_Ticket(1, triad[01], nn, sn, ans[01], denomination);
            var t02 =  get_Ticket(2, triad[02], nn, sn, ans[02], denomination);

            var taskList = new List<Task> { t00, t01, t02 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            try
            {
                CoreLogger.Log(sn + " get ticket:" + triad[00] + " " + responseArray[triad[00]].fullResponse);
                CoreLogger.Log(sn + " get ticket:" + triad[01] + " " + responseArray[triad[01]].fullResponse);
                CoreLogger.Log(sn + " get ticket:" + triad[02] + " " + responseArray[triad[02]].fullResponse);
            }
            catch { }
            //Get data from the detection agents
        }//end detect coin



    }//end RAIDA Class
}// End Namespace
