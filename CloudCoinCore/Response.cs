using System;

namespace Founders
{
    public class Response
    {
        public string fullRequest;
        public string fullResponse;
        public bool success;
        public string outcome;
        public int milliseconds;


        public Response(bool success, string outcome, int milliseconds, string fullRequest, string fullResponse)
        {
            this.success = success;
            this.outcome = outcome;
            this.milliseconds = milliseconds;
            this.fullRequest = fullRequest;
            this.fullResponse = fullResponse;
        }
        public Response()
        {
            this.success = false;
            this.outcome = "not used";
            this.milliseconds = 0;
            this.fullRequest = "No request";
            this.fullResponse = "No response";
        }
    }
}
