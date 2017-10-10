
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
        public Response[,] responseArrayMulti;

        private int[] working_triad = { 0, 1, 2 };//place holder
        public bool[] raidaIsDetecting = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
        public string[] lastDetectStatus = { "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected" };
        public string[] echoStatus = { "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply" };

        public static string[] countries = new String[] { "Australia", "Macedonia", "Philippines", "Serbia", "Bulgaria", "Russia", "Switzerland", "United Kingdom", "Punjab", "India", "Croatia", "USA", "India", "Taiwan", "Moscow", "St.Petersburg", "Columbia", "Singapore", "Germany", "Canada", "Venezuela", "Hyperbad", "USA", "Ukraine", "Luxenburg" };


        /* CONSTRUCTOR */
        public RAIDA()
        { //  initialise instance variables
            this.agent = new DetectionAgent[25];

            for (int i = 0; (i < 25); i++)
            {
                this.agent[i] = new DetectionAgent(i);
                
            } // end for each Raida
        }//End Constructor


        public async Task echoOne(int raida_id)
        {
            DetectionAgent da = new DetectionAgent(raida_id);
            responseArray[raida_id] = await da.echo(raida_id);


            Console.Write(".");
        }//end echo 




        public Response[] echoAll(int milliSecondsToTimeOut)
        {


            Response[] results = new Response[25];
            var t00 = echoOne(00);
            var t01 = echoOne(01);
            var t02 = echoOne(02);
            var t03 = echoOne(03);
            var t04 = echoOne(04);
            var t05 = echoOne(05);
            var t06 = echoOne(06);
            var t07 = echoOne(07);
            var t08 = echoOne(08);
            var t09 = echoOne(09);
            var t10 = echoOne(10);
            var t11 = echoOne(11);
            var t12 = echoOne(12);
            var t13 = echoOne(13);
            var t14 = echoOne(14);
            var t15 = echoOne(15);
            var t16 = echoOne(16);
            var t17 = echoOne(17);
            var t18 = echoOne(18);
            var t19 = echoOne(19);
            var t20 = echoOne(20);
            var t21 = echoOne(21);
            var t22 = echoOne(22);
            var t23 = echoOne(23);
            var t24 = echoOne(24);

            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24 };
            Task.WaitAll(taskList.ToArray(), milliSecondsToTimeOut);
            for (int i = 0; i < 25; i++)
                if (responseArray[i] != null)
                    CoreLogger.Log("echo:" + i + " " + responseArray[i].fullResponse);


            return results;
        }//end echo



        public async Task detectOne(int raida_id, int nn, int sn, String an, String pan, int d)
        {
            DetectionAgent da = new DetectionAgent(raida_id);
            responseArray[raida_id] = await da.detect(nn, sn, an, pan, d);
        }//end detectOne


        /* For Multi-Detect*/
        public async Task detectOneMulti(int raida_id, int[] nn, int[] sn, String[] an, String[] pan, int[] d, int milliSecondsToTimeOut)
        {
            DetectionAgent da = new DetectionAgent( raida_id );

            Response[] tempArray = await da.multiDetect(nn, sn, an, pan, d, milliSecondsToTimeOut);

            for(int i=0; i< tempArray.Length; i++)//Fill up the array
            { 
                responseArrayMulti[raida_id,i] = tempArray[i];
            }

        }//end detectOneMulti

        public CoinUtils[] detectMultiCoin(CoinUtils[] cu, int milliSecondsToTimeOut)
        {
            //Make arrays to stripe the coins

            responseArrayMulti = new Response[25, cu.Length];

            int[] nns = new int[cu.Length];
            int[] sns = new int[cu.Length];
            String[] ans_0 = new String[cu.Length];
            String[] ans_1 = new String[cu.Length];
            String[] ans_2 = new String[cu.Length];
            String[] ans_3 = new String[cu.Length];
            String[] ans_4 = new String[cu.Length];
            String[] ans_5 = new String[cu.Length];
            String[] ans_6 = new String[cu.Length];
            String[] ans_7 = new String[cu.Length];
            String[] ans_8 = new String[cu.Length];
            String[] ans_9 = new String[cu.Length];
            String[] ans_10 = new String[cu.Length];
            String[] ans_11 = new String[cu.Length];
            String[] ans_12 = new String[cu.Length];
            String[] ans_13 = new String[cu.Length];
            String[] ans_14 = new String[cu.Length];
            String[] ans_15 = new String[cu.Length];
            String[] ans_16 = new String[cu.Length];
            String[] ans_17 = new String[cu.Length];
            String[] ans_18 = new String[cu.Length];
            String[] ans_19 = new String[cu.Length];
            String[] ans_20 = new String[cu.Length];
            String[] ans_21 = new String[cu.Length];
            String[] ans_22 = new String[cu.Length];
            String[] ans_23 = new String[cu.Length];
            String[] ans_24 = new String[cu.Length];
            String[] pans_0 = new String[cu.Length];
            String[] pans_1 = new String[cu.Length];
            String[] pans_2 = new String[cu.Length];
            String[] pans_3 = new String[cu.Length];
            String[] pans_4 = new String[cu.Length];
            String[] pans_5 = new String[cu.Length];
            String[] pans_6 = new String[cu.Length];
            String[] pans_7 = new String[cu.Length];
            String[] pans_8 = new String[cu.Length];
            String[] pans_9 = new String[cu.Length];
            String[] pans_10 = new String[cu.Length];
            String[] pans_11 = new String[cu.Length];
            String[] pans_12 = new String[cu.Length];
            String[] pans_13 = new String[cu.Length];
            String[] pans_14 = new String[cu.Length];
            String[] pans_15 = new String[cu.Length];
            String[] pans_16 = new String[cu.Length];
            String[] pans_17 = new String[cu.Length];
            String[] pans_18 = new String[cu.Length];
            String[] pans_19 = new String[cu.Length];
            String[] pans_20 = new String[cu.Length];
            String[] pans_21 = new String[cu.Length];
            String[] pans_22 = new String[cu.Length];
            String[] pans_23 = new String[cu.Length];
            String[] pans_24 = new String[cu.Length];

            int[] dens = new int[cu.Length];//Denominations
            //Stripe the coins
     
            for (int i = 0; i < cu.Length; i++)//For every coin
            {
                cu[i].generatePan();
                nns[i] = cu[i].cc.nn;
                sns[i] = cu[i].cc.sn;
                ans_0[i] = cu[i].cc.an[0];
                ans_1[i] = cu[i].cc.an[1];
                ans_2[i] = cu[i].cc.an[2];
                ans_3[i] = cu[i].cc.an[3];
                ans_4[i] = cu[i].cc.an[4];
                ans_5[i] = cu[i].cc.an[5];
                ans_6[i] = cu[i].cc.an[6];
                ans_7[i] = cu[i].cc.an[7];
                ans_8[i] = cu[i].cc.an[8];
                ans_9[i] = cu[i].cc.an[9];
                ans_10[i] = cu[i].cc.an[10];
                ans_11[i] = cu[i].cc.an[11];
                ans_12[i] = cu[i].cc.an[12];
                ans_13[i] = cu[i].cc.an[13];
                ans_14[i] = cu[i].cc.an[14];
                ans_15[i] = cu[i].cc.an[15];
                ans_16[i] = cu[i].cc.an[16];
                ans_17[i] = cu[i].cc.an[17];
                ans_18[i] = cu[i].cc.an[18];
                ans_19[i] = cu[i].cc.an[19];
                ans_20[i] = cu[i].cc.an[20];
                ans_21[i] = cu[i].cc.an[21];
                ans_22[i] = cu[i].cc.an[22];
                ans_23[i] = cu[i].cc.an[23];
                ans_24[i] = cu[i].cc.an[24];

                pans_0[i] = cu[i].pans[0];
                pans_1[i] = cu[i].pans[1];
                pans_2[i] = cu[i].pans[2];
                pans_3[i] = cu[i].pans[3];
                pans_4[i] = cu[i].pans[4];
                pans_5[i] = cu[i].pans[5];
                pans_6[i] = cu[i].pans[6];
                pans_7[i] = cu[i].pans[7];
                pans_8[i] = cu[i].pans[8];
                pans_9[i] = cu[i].pans[9];
                pans_10[i] = cu[i].pans[10];
                pans_11[i] = cu[i].pans[11];
                pans_12[i] = cu[i].pans[12];
                pans_13[i] = cu[i].pans[13];
                pans_14[i] = cu[i].pans[14];
                pans_15[i] = cu[i].pans[15];
                pans_16[i] = cu[i].pans[16];
                pans_17[i] = cu[i].pans[17];
                pans_18[i] = cu[i].pans[18];
                pans_19[i] = cu[i].pans[19];
                pans_20[i] = cu[i].pans[20];
                pans_21[i] = cu[i].pans[21];
                pans_22[i] = cu[i].pans[22];
                pans_23[i] = cu[i].pans[23];
                pans_24[i] = cu[i].pans[24];

                dens[i] = cu[i].getDenomination();
            }//end for every coin put in an array

            var t00 = detectOneMulti(00, nns, sns, ans_0, pans_0, dens, milliSecondsToTimeOut);
            var t01 = detectOneMulti(01, nns, sns, ans_1, pans_1, dens, milliSecondsToTimeOut);
            var t02 = detectOneMulti(02, nns, sns, ans_2, pans_2, dens, milliSecondsToTimeOut);
            var t03 = detectOneMulti(03, nns, sns, ans_3, pans_3, dens, milliSecondsToTimeOut);
            var t04 = detectOneMulti(04, nns, sns, ans_4, pans_4, dens, milliSecondsToTimeOut);
            var t05 = detectOneMulti(05, nns, sns, ans_5, pans_5, dens, milliSecondsToTimeOut);
            var t06 = detectOneMulti(06, nns, sns, ans_6, pans_6, dens, milliSecondsToTimeOut);
            var t07 = detectOneMulti(07, nns, sns, ans_7, pans_7, dens, milliSecondsToTimeOut);
            var t08 = detectOneMulti(08, nns, sns, ans_8, pans_8, dens, milliSecondsToTimeOut);
            var t09 = detectOneMulti(09, nns, sns, ans_9, pans_9, dens, milliSecondsToTimeOut);
            var t10 = detectOneMulti(10, nns, sns, ans_10, pans_10, dens, milliSecondsToTimeOut);
            var t11 = detectOneMulti(11, nns, sns, ans_11, pans_11, dens, milliSecondsToTimeOut);
            var t12 = detectOneMulti(12, nns, sns, ans_12, pans_12, dens, milliSecondsToTimeOut);
            var t13 = detectOneMulti(13, nns, sns, ans_13, pans_13, dens, milliSecondsToTimeOut);
            var t14 = detectOneMulti(14, nns, sns, ans_14, pans_14, dens, milliSecondsToTimeOut);
            var t15 = detectOneMulti(15, nns, sns, ans_15, pans_15, dens, milliSecondsToTimeOut);
            var t16 = detectOneMulti(16, nns, sns, ans_16, pans_16, dens, milliSecondsToTimeOut);
            var t17 = detectOneMulti(17, nns, sns, ans_17, pans_17, dens, milliSecondsToTimeOut);
            var t18 = detectOneMulti(18, nns, sns, ans_18, pans_18, dens, milliSecondsToTimeOut);
            var t19 = detectOneMulti(19, nns, sns, ans_19, pans_19, dens, milliSecondsToTimeOut);
            var t20 = detectOneMulti(20, nns, sns, ans_20, pans_20, dens, milliSecondsToTimeOut);
            var t21 = detectOneMulti(21, nns, sns, ans_21, pans_21, dens, milliSecondsToTimeOut);
            var t22 = detectOneMulti(22, nns, sns, ans_22, pans_22, dens, milliSecondsToTimeOut);
            var t23 = detectOneMulti(23, nns, sns, ans_23, pans_23, dens, milliSecondsToTimeOut);
            var t24 = detectOneMulti(24, nns, sns, ans_24, pans_24, dens, milliSecondsToTimeOut);


            var taskList = new List<Task> { t00, t01, t02, t03, t04, t05, t06, t07, t08, t09, t10, t11, t12, t13, t14, t15, t16, t17, t18, t19, t20, t21, t22, t23, t24 };

            Task.WaitAll( taskList.ToArray(), milliSecondsToTimeOut );
            
            //Get data from the detection agents
            for (int i = 0; i < nns.Length; i++)
            {
                for (int j = 0; j<25; j++) {//For each coin
                    if (responseArrayMulti[j, i] != null)
                    {
                        cu[i].setPastStatus(responseArrayMulti[j, i].outcome, j);
                        CoreLogger.Log(cu[i].cc.sn + " detect:" + j + " " + responseArrayMulti[j, i].fullResponse);
                    }
                    else
                    {
                        cu[i].setPastStatus("undetected", j);
                    };// should be pass, fail, error or undetected, or No response. 
                }//end for each coin checked

                cu[i].setAnsToPansIfPassed();
                cu[i].calculateHP();
                cu[i].calcExpirationDate();
                cu[i].grade();
            }//end for each detection agent

            return cu;//Return the array of coins detected
        }//end detect coin



        public CoinUtils detectCoin(CoinUtils cu, int milliSecondsToTimeOut)
        {
            cu.generatePan();

            var t00 = detectOne(00, cu.cc.nn, cu.cc.sn, cu.cc.an[00], cu.pans[00], cu.getDenomination());
            var t01 = detectOne(01, cu.cc.nn, cu.cc.sn, cu.cc.an[01], cu.pans[01], cu.getDenomination());
            var t02 = detectOne(02, cu.cc.nn, cu.cc.sn, cu.cc.an[02], cu.pans[02], cu.getDenomination());
            var t03 = detectOne(03, cu.cc.nn, cu.cc.sn, cu.cc.an[03], cu.pans[03], cu.getDenomination());
            var t04 = detectOne(04, cu.cc.nn, cu.cc.sn, cu.cc.an[04], cu.pans[04], cu.getDenomination());
            var t05 = detectOne(05, cu.cc.nn, cu.cc.sn, cu.cc.an[05], cu.pans[05], cu.getDenomination());
            var t06 = detectOne(06, cu.cc.nn, cu.cc.sn, cu.cc.an[06], cu.pans[06], cu.getDenomination());
            var t07 = detectOne(07, cu.cc.nn, cu.cc.sn, cu.cc.an[07], cu.pans[07], cu.getDenomination());
            var t08 = detectOne(08, cu.cc.nn, cu.cc.sn, cu.cc.an[08], cu.pans[08], cu.getDenomination());
            var t09 = detectOne(09, cu.cc.nn, cu.cc.sn, cu.cc.an[09], cu.pans[09], cu.getDenomination());
            var t10 = detectOne(10, cu.cc.nn, cu.cc.sn, cu.cc.an[10], cu.pans[10], cu.getDenomination());
            var t11 = detectOne(11, cu.cc.nn, cu.cc.sn, cu.cc.an[11], cu.pans[11], cu.getDenomination());
            var t12 = detectOne(12, cu.cc.nn, cu.cc.sn, cu.cc.an[12], cu.pans[12], cu.getDenomination());
            var t13 = detectOne(13, cu.cc.nn, cu.cc.sn, cu.cc.an[13], cu.pans[13], cu.getDenomination());
            var t14 = detectOne(14, cu.cc.nn, cu.cc.sn, cu.cc.an[14], cu.pans[14], cu.getDenomination());
            var t15 = detectOne(15, cu.cc.nn, cu.cc.sn, cu.cc.an[15], cu.pans[15], cu.getDenomination());
            var t16 = detectOne(16, cu.cc.nn, cu.cc.sn, cu.cc.an[16], cu.pans[16], cu.getDenomination());
            var t17 = detectOne(17, cu.cc.nn, cu.cc.sn, cu.cc.an[17], cu.pans[17], cu.getDenomination());
            var t18 = detectOne(18, cu.cc.nn, cu.cc.sn, cu.cc.an[18], cu.pans[18], cu.getDenomination());
            var t19 = detectOne(19, cu.cc.nn, cu.cc.sn, cu.cc.an[19], cu.pans[19], cu.getDenomination());
            var t20 = detectOne(20, cu.cc.nn, cu.cc.sn, cu.cc.an[20], cu.pans[20], cu.getDenomination());
            var t21 = detectOne(21, cu.cc.nn, cu.cc.sn, cu.cc.an[21], cu.pans[21], cu.getDenomination());
            var t22 = detectOne(22, cu.cc.nn, cu.cc.sn, cu.cc.an[22], cu.pans[22], cu.getDenomination());
            var t23 = detectOne(23, cu.cc.nn, cu.cc.sn, cu.cc.an[23], cu.pans[23], cu.getDenomination());
            var t24 = detectOne(24, cu.cc.nn, cu.cc.sn, cu.cc.an[24], cu.pans[24], cu.getDenomination());


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


        public async Task get_Ticket(int i, int raidaID, int nn, int sn, String an, int d, int millisecondsToTimeout)
        {
            DetectionAgent da = new DetectionAgent(raidaID);
            responseArray[raidaID] = await da.get_ticket(nn, sn, an, d, millisecondsToTimeout);


        }//end get ticket


        public void get_Tickets(int[] triad, String[] ans, int nn, int sn, int denomination, int millisecondsToTimeout)
        {
            //Empty the status of any old ticket info. 
            RAIDA_Status.resetTickets();
            //Console.WriteLine("Get Tickets called. ");
            var t00 = get_Ticket(0, triad[00], nn, sn, ans[00], denomination, millisecondsToTimeout);
            var t01 = get_Ticket(1, triad[01], nn, sn, ans[01], denomination, millisecondsToTimeout);
            var t02 = get_Ticket(2, triad[02], nn, sn, ans[02], denomination, millisecondsToTimeout);

            var taskList = new List<Task> { t00, t01, t02 };
            Task.WaitAll(taskList.ToArray(), millisecondsToTimeout);
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
