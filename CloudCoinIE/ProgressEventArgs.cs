using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinInvestors
{
    public class ProgressEventArgs : EventArgs
    {
        public string Status { get; private set; }
        public int percentage { get; private set; }
        public ProgressEventArgs(string status,int percentage =0)
        {
            Status = status;
            this.percentage = percentage;
        }


    }
}
