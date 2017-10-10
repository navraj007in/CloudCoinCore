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
        public int RAIDANumber;
        public String fullUrl;

        //Constructor
        public DetectionAgent(int RAIDANumber)
        {
            this.RAIDANumber = RAIDANumber;
            fullUrl = "https://RAIDA" + RAIDANumber + ".cloudcoin.global/service/";
        }//Detection Agent Constructor

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

        public async Task<Response[]> multiDetect(int[] nn, int[] sn, String[] an, String[] pan, int[] d, int timeout)
        {
            /*PREPARE REQUEST*/
            Response[] response = new Response[nn.Length];
            for (int i = 0; i < nn.Length; i++)
            {
                response[i] = new Response();
            }

            //Create List of KeyValuePairs to use as the POST data
            List<KeyValuePair<string, string>> postVariables = new List<KeyValuePair<string, string>>();

            //Loop over String array and add all instances to our bodyPoperties
            for (int i = 0; i < nn.Length; i++)
            {
                postVariables.Add(new KeyValuePair<string, string>("nns[]", nn[i].ToString()));
                postVariables.Add(new KeyValuePair<string, string>("sns[]", sn[i].ToString()));
                postVariables.Add(new KeyValuePair<string, string>("ans[]", an[i]));
                postVariables.Add(new KeyValuePair<string, string>("pans[]", pan[i]));
                postVariables.Add(new KeyValuePair<string, string>("denomination[]", d[i].ToString()));
                //Console.Out.WriteLine("url is " + this.fullUrl + "detect?nns[]=" + nn[i] + "&sns[]=" + sn[i] + "&ans[]=" + an[i] + "&pans[]=" + pan[i] + "&denomination[]=" + d[i]);

                response[i].fullRequest = this.fullUrl + "detect?nns[]=" + nn[i] + "&sns[]=" + sn[i] + "&ans[]=" + an[i] + "&pans[]=" + pan[i] + "&denomination[]=" + d[i];//Record what was sent
            }

            //convert postVariables to an object of FormUrlEncodedContent
            var dataContent = new FormUrlEncodedContent(postVariables.ToArray());
            DateTime before = DateTime.Now;
            DateTime after;
            TimeSpan ts = new TimeSpan();


            /*MAKE REQEST*/
            string totalResponse = "";
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);
    
            try
            {
                //POST THE REQUEST AND FILL THE ANSER IN totalResponse
                totalResponse = "";
                HttpResponseMessage json;

                using (client)
                {
                   // Console.Write("postHtml await for response: ");
                    json = await client.PostAsync(fullUrl + "multi_detect", dataContent);

                    //Console.Write(".");
                    if (json.IsSuccessStatusCode)//200 status good
                    {
                        totalResponse = await json.Content.ReadAsStringAsync();
                        Console.Out.WriteLine("RAIDA " + RAIDANumber + " returned good: " + json.StatusCode);
                      //  Console.Out.WriteLine(totalResponse);
                    }
                    else //404 not found or 500 error. 
                    {
                        Console.Out.WriteLine( "RAIDA "+ RAIDANumber + " had an error: " + json.StatusCode);
                        after = DateTime.Now;
                        ts = after.Subtract(before);//Start the timer
                        for (int i = 0; i < nn.Length; i++)
                        {
                            response[i].outcome = "error";
                            response[i].fullResponse = json.StatusCode.ToString() ;
                            response[i].success = false;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                            RAIDA_Status.failsDetect[RAIDANumber] = true;
                        }//end for every CloudCoin note
                        return response;//END IF THE REQUEST GOT AN ERROR

                    }//end else 404 or 500
                        
                }//end using

            }
            catch (TaskCanceledException ex)//This means it timed out
            {
               // Console.WriteLine("T1:" + ex.Message);
                after = DateTime.Now;
                ts = after.Subtract(before);//Start the timer
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i].outcome = "noresponse";
                    response[i].fullResponse = ex.Message;
                    response[i].success = false;
                    response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    RAIDA_Status.failsDetect[RAIDANumber] = true;
                }//end for every CloudCoin note
                return response;//END IF THE REQUEST FAILED
            }
            catch (Exception ex)//Request failed with some kind of error that did not provide a response. 
            {
               // Console.WriteLine("M1:" + ex.Message);
                after = DateTime.Now;
                ts = after.Subtract(before);//Start the timer
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i].outcome = "error";
                    response[i].fullResponse = ex.Message;
                    response[i].success = false;
                    response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    RAIDA_Status.failsDetect[RAIDANumber] = true;
                }//end for every CloudCoin note
                return response;//END IF THE REQUEST FAILED
            }//end catch request attmept


            /* PROCESS REQUEST*/
            after = DateTime.Now;
            ts = after.Subtract(before);//Start the timer
            //Is the request a dud?
            if (totalResponse.Contains("dud"))
            {
                //Mark all Responses as duds
                for (int i = 0; i < nn.Length; i++)
                {
                    response[i].fullResponse = totalResponse;
                    response[i].success = false;
                    response[i].outcome = "dud";
                    response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
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
                string[] responseArray = Regex.Split(totalResponse, "},{");
                //Check to see if the responseArray is the same length as the request detectResponse. They should be the same
                if (response.Length != responseArray.Length)
                {
                    //Mark all Responses as duds
                    for (int i = 0; i < nn.Length; i++)
                    {
                        response[i].fullResponse = totalResponse;
                        response[i].success = false;
                        response[i].outcome = "dud";
                        response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                    }//end for each dud
                }//end if lenghts are not the same
                else//Lengths are the same so lets go through each one
                {


                    for (int i = 0; i < nn.Length; i++)
                    {
                        if (responseArray[i].Contains("pass"))
                        {
                            response[i].fullResponse = responseArray[i];
                            response[i].outcome = "pass";
                            response[i].success = true;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }
                        else if (responseArray[i].Contains("fail") && responseArray[i].Length < 200)//less than 200 incase there is a fail message inside errored page
                        {
                            response[i].fullResponse = responseArray[i];
                            response[i].outcome = "fail";
                            response[i].success = false;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }
                        else
                        {
                            response[i].fullResponse = responseArray[i];
                            response[i].outcome = "error";
                            response[i].success = false;
                            response[i].milliseconds = Convert.ToInt32(ts.Milliseconds);
                        }
                    }//End for each response
                }//end if array lengths are the same

            }//End Else not a dud
             //Break the respons into sub responses. 
            RAIDA_Status.multiDetectTime[RAIDANumber] = Convert.ToInt32(ts.Milliseconds);

            return response;
        }//End multi detect



         //Sends a Detection request to a RAIDA server
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

        //Returns an ticket from a trusted server
        public async Task<Response> get_ticket(int nn, int sn, String an, int d, int millisecondsToTimeout)
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

         //Repairs a fracked RAIDA
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

        //Method getHtml download a webpage or a web service
        private async Task<String> getHtml(String urlAddress)
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

        // Method ordinalIndexOf used to parse cloudcoins. Finds the nth number of a character within a string
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
