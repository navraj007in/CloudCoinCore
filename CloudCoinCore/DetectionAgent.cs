using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace Founders
{
    public class DetectionAgent
    {
        public int readTimeout;
        public int RAIDANumber;
        public String fullUrl;


        /**
        * DetectionAgent Constructor
        * @param readTimeout A parameter that determines how many milliseconds each request will be allowed to take
        * @param RAIDANumber The number of the RAIDA server 0-24
        */
        public DetectionAgent(int RAIDANumber, int readTimeout)
        {
            this.RAIDANumber = RAIDANumber;
            fullUrl = "https://RAIDA" + RAIDANumber + ".cloudcoin.global/service/";
            this.readTimeout = readTimeout;
        }//Detection Agent Constructor



        /**
        * Method ECHO
        * @param raidaID The number of the RAIDA server 0-24
        */
        public async Task<Response> echo(int raidaID)
        {
            Response echoResponse = new Response();
            echoResponse.fullRequest = this.fullUrl + "echo?b=t";
            DateTime before = DateTime.Now;
            RAIDA_Status.failsEcho[raidaID] = true;
            try
            {
                echoResponse.fullResponse = await getHtml(echoResponse.fullRequest);
                if (echoResponse.fullResponse.Contains("ready"))
                {
                    echoResponse.success = true;
                    echoResponse.outcome = "ready";
                    RAIDA_Status.failsEcho[raidaID] = false;
                }
                else
                {
                    echoResponse.success = false;
                    echoResponse.outcome = "error";
                    RAIDA_Status.failsEcho[raidaID] = true;
                }
            }
            catch (Exception ex)
            {
                echoResponse.outcome = "error";
                echoResponse.success = false;
                RAIDA_Status.failsEcho[raidaID] = true;
                if (ex.InnerException != null)
                    echoResponse.fullResponse = ex.InnerException.Message;
            }
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
            echoResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);
            RAIDA_Status.echoTime[raidaID] = Convert.ToInt32(ts.Milliseconds);
            //Console.WriteLine("RAIDA # " + raidaID + RAIDA_Status.failsEcho[raidaID]);

            return echoResponse;
        }//end detect

        public async Task<Response[]> multiDetect(int[] nn, int[] sn, String[] an, String[] pan, int[] d)
        {
            Response[] detectResponse = new Response[ nn.Length ];

            //Create List of KeyValuePairs to use as the POST data
            List<KeyValuePair<string, string>> postVariables = new List<KeyValuePair<string, string>>();


            //Loop over String array and add all instances to our bodyPoperties
            for (int i = 0; i < nn.Length; i++)
            {
                postVariables.Add(new KeyValuePair<string, string>("nn[]", nn[i].ToString()));
                postVariables.Add(new KeyValuePair<string, string>("sn[]", sn[i].ToString()));
                postVariables.Add(new KeyValuePair<string, string>("an[]", an[i]));
                postVariables.Add(new KeyValuePair<string, string>("pan[]", pan[i]));
                postVariables.Add(new KeyValuePair<string, string>("denomination[]", d[i].ToString()));

                detectResponse[i].fullRequest = this.fullUrl + "detect?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + pan + "&denomination=" + d;//Record what was sent
            }

            //convert postVariables to an object of FormUrlEncodedContent
            var dataContent = new FormUrlEncodedContent(postVariables.ToArray());
            DateTime before = DateTime.Now;
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);//Start the timer
            try
            {
                //POST THE REQUEST AND FILL THE ANSER IN totalResponse
                
                string totalResponse = await postHtml( fullUrl + "multi_detect.aspx", dataContent);

                //Is the request a dud?
                if (totalResponse.Contains("dud")) {
                    //Mark all Responses as duds
                    for (int i = 0; i < nn.Length; i++)
                    {
                        detectResponse[i].fullResponse = totalResponse;
                        detectResponse[i].success = false;
                        detectResponse[i].outcome = "dud";
                        detectResponse[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    }//end for each dud
                }//end if dud
                else
                {
                    //Not a dud so break up parts into smaller pieces
                    //Remove leading "[{"
                    totalResponse = totalResponse.Remove(0, 2);
                    //Remove trailing "}]"
                    totalResponse = totalResponse.Remove(totalResponse.Length - 2, 2);
                    //Split by "},{"
                    string[] responseArray = Regex.Split(totalResponse,"},{");
                    //Check to see if the responseArray is the same length as the request detectResponse. They should be the same
                    if (detectResponse.Length != responseArray.Length) {
                        //Mark all Responses as duds
                        for (int i = 0; i < nn.Length; i++)
                        {
                            detectResponse[i].fullResponse = totalResponse;
                            detectResponse[i].success = false;
                            detectResponse[i].outcome = "dud";
                            detectResponse[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }//end for each dud
                    }//end if lenghts are not the same
                    else//Lengths are the same so lets go through each one
                    {


                        for (int i = 0; i < nn.Length; i++)
                        {
                            if (responseArray[i].Contains("pass"))
                            {
                                detectResponse[i].outcome = "pass";
                                detectResponse[i].success = true;
                                detectResponse[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                            }
                            else if ( responseArray[i].Contains("fail") && responseArray[i].Length < 200)//less than 200 incase their is a fail message inside errored page
                            {
                                detectResponse[i].outcome = "fail";
                                detectResponse[i].success = false;
                                detectResponse[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                            }
                            else
                            {
                                detectResponse[i].outcome = "error";
                                detectResponse[i].success = false;
                                detectResponse[i].milliseconds = Convert.ToInt32(ts.Milliseconds);       
                            }
                        }//End for each response
                    }//end if array lengths are the same
                   
                }//End Else not a dud
                //Break the respons into sub responses. 


            }
            catch (Exception ex)//Request failed
            {
                for (int i = 0; i < nn.Length; i++)
                {
                    detectResponse[i].outcome = "error";
                    detectResponse[i].fullResponse = ex.InnerException.Message;
                    detectResponse[i].success = false;
                    detectResponse[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    RAIDA_Status.failsDetect[RAIDANumber] = true;
                }//end for every CloudCoin note
            }
            return detectResponse;


    }//End multi detect


            /**
             * Method DETECT
             * Sends a Detection request to a RAIDA server
             * @param nn  int that is the coin's Network Number 
             * @param sn  int that is the coin's Serial Number
             * @param an String that is the coin's Authenticity Number (GUID)
             * @param pan String that is the Proposed Authenticity Number to replace the AN.
             * @param d int that is the Denomination of the Coin
             * @return Response object. 
             */
            public async Task<Response> detect(int nn, int sn, String an, String pan, int d)
        {
            Response detectResponse = new Response();
            detectResponse.fullRequest = this.fullUrl + "detect?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + pan + "&denomination=" + d + "&b=t";
            DateTime before = DateTime.Now;
            try
            {
                detectResponse.fullResponse = await getHtml(detectResponse.fullRequest);
                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                detectResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);

                if (detectResponse.fullResponse.Contains("pass"))
                {
                    detectResponse.outcome = "pass";
                    detectResponse.success = true;
                }
                else if (detectResponse.fullResponse.Contains("fail") && detectResponse.fullResponse.Length < 200)//less than 200 incase their is a fail message inside errored page
                {
                    detectResponse.outcome = "fail";
                    detectResponse.success = false;
                    RAIDA_Status.failsDetect[RAIDANumber] = true;
                }
                else
                {
                    detectResponse.outcome = "error";
                    detectResponse.success = false;
                    RAIDA_Status.failsDetect[RAIDANumber] = true;
                }

            }
            catch (Exception ex)
            {
                detectResponse.outcome = "error";
                detectResponse.fullResponse = ex.InnerException.Message;
                detectResponse.success = false;
                RAIDA_Status.failsDetect[RAIDANumber] = true;
            }
            return detectResponse;
        }//end detect



        /**
        * Method GET TICKET
        * Returns an ticket from a trusted server
        * @param nn  int that is the coin's Network Number 
        * @param sn  int that is the coin's Serial Number
        * @param an String that is the coin's Authenticity Number (GUID)
        * @param pan String that is the Proposed Authenticity Number to replace the AN.
        * @param d int that is the Denomination of the Coin
        * @return Response object. 
        */
        public async Task<Response> get_ticket(int nn, int sn, String an, int d)
        {
            Response get_ticketResponse = new Response();
            get_ticketResponse.fullRequest = fullUrl + "get_ticket?nn=" + nn + "&sn=" + sn + "&an=" + an + "&pan=" + an + "&denomination=" + d;
            DateTime before = DateTime.Now;

            try
            {
                get_ticketResponse.fullResponse = await getHtml(get_ticketResponse.fullRequest);
                DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
                get_ticketResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);

                if (get_ticketResponse.fullResponse.Contains("ticket"))
                {
                    String[] KeyPairs = get_ticketResponse.fullResponse.Split(',');
                    String message = KeyPairs[3];
                    int startTicket = ordinalIndexOf(message, "\"", 3) + 2;
                    int endTicket = ordinalIndexOf(message, "\"", 4) - startTicket;
                    get_ticketResponse.outcome = message.Substring(startTicket - 1, endTicket + 1); //This is the ticket or message
                    get_ticketResponse.success = true;
                    RAIDA_Status.hasTicket[RAIDANumber] = true;
                    RAIDA_Status.ticketHistory[RAIDANumber] = RAIDA_Status.TicketHistory.Success;
                    RAIDA_Status.tickets[RAIDANumber] = get_ticketResponse.outcome;

                }
                else
                {
                    get_ticketResponse.success = false;
                    RAIDA_Status.hasTicket[RAIDANumber] = false;
                    RAIDA_Status.ticketHistory[RAIDANumber] = RAIDA_Status.TicketHistory.Failed;
                }//end if

            }
            catch (Exception ex)
            {
                get_ticketResponse.outcome = "error";
                get_ticketResponse.fullResponse = ex.InnerException.Message;
                get_ticketResponse.success = false;
                RAIDA_Status.hasTicket[RAIDANumber] = false;
                RAIDA_Status.ticketHistory[RAIDANumber] = RAIDA_Status.TicketHistory.Failed;
            }//end try catch
            return get_ticketResponse;
        }//end get ticket



        /**
         * Method FIX
         * Repairs a fracked RAIDA
         * @param triad three ints trusted server RAIDA numbers
         * @param m1 string ticket from the first trusted server
         * @param m2 string ticket from the second trusted server
         * @param m3 string ticket from the third trusted server
         * @param pan string proposed authenticity number (to replace the wrong AN the RAIDA has)
         * @return string status sent back from the server: sucess, fail or error. 
         */
        public async Task<Response> fix(int[] triad, String m1, String m2, String m3, String pan)
        {
            Response fixResponse = new Response();
            DateTime before = DateTime.Now;
            fixResponse.fullRequest = fullUrl + "fix?fromserver1=" + triad[0] + "&message1=" + m1 + "&fromserver2=" + triad[1] + "&message2=" + m2 + "&fromserver3=" + triad[2] + "&message3=" + m3 + "&pan=" + pan;
            DateTime after = DateTime.Now; TimeSpan ts = after.Subtract(before);
            fixResponse.milliseconds = Convert.ToInt32(ts.Milliseconds);

            try
            {
                fixResponse.fullResponse = await getHtml(fixResponse.fullRequest);
                if (fixResponse.fullResponse.Contains("success"))
                {
                    fixResponse.outcome = "success";
                    fixResponse.success = true;
                }
                else
                {
                    fixResponse.outcome = "fail";
                    fixResponse.success = false;
                }
            }
            catch (Exception ex)
            {//quit
                fixResponse.outcome = "error";
                fixResponse.fullResponse = ex.InnerException.Message;
                fixResponse.success = false;
            }
            return fixResponse;
        }//end fixit




        /**
         * Method getHtml download a webpage or a web service
         *
         * @param url_in The URL to be downloaded
         * @return The text that was downloaded
         */
        private async Task<String> getHtml( String urlAddress )
        {
            //Console.Out.WriteLine(urlAddress);

            // Console.Out.Write(".");
            string data = "";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            //request.ContinueTimeout = readTimeout;
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11";

            try
            {
                using (var cli = new HttpClient())
                {
                    HttpResponseMessage response = await cli.GetAsync(urlAddress);

                    //Console.Write(".");
                    if (response.IsSuccessStatusCode)
                        data = await response.Content.ReadAsStringAsync();
                    // System.Console.Out.WriteLine(data);  
                }
            }
            catch (Exception ex)
            {
                // Console.Out.WriteLine(ex.Message);

                return ex.Message;
            }
            // Console.Out.WriteLine(data);
            return data;
        }//end get HTML

        private async Task<String> postHtml(String urlAddress, FormUrlEncodedContent postContent)
        {
            //Console.Out.WriteLine(urlAddress);

            // Console.Out.Write(".");
            string data = "";
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            //request.ContinueTimeout = readTimeout;
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11";

            try
            {
                using (var cli = new HttpClient())
                {
                    HttpResponseMessage response = await cli.PostAsync(urlAddress, postContent);

                    //Console.Write(".");
                    if (response.IsSuccessStatusCode)
                        data = await response.Content.ReadAsStringAsync();
                    // System.Console.Out.WriteLine(data);  
                }
            }
            catch (Exception ex)
            {
                // Console.Out.WriteLine(ex.Message);

                return ex.Message;
            }
            // Console.Out.WriteLine(data);
            return data;
        }//end get HTML



        /**
         * Method ordinalIndexOf used to parse cloudcoins. Finds the nth number of a character within a string
         *
         * @param str The string to search in
         * @param substr What to count in the string
         * @param n The nth number
         * @return The index of the nth number
         */
        public int ordinalIndexOf(string str, string substr, int n)
        {
            int pos = str.IndexOf(substr);
            while (--n > 0 && pos != -1)
            {
                pos = str.IndexOf(substr, (pos + 1));
            }
            return pos;
        }//end ordinal Index of

    }
}
