using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;
using System.IO;

namespace CloudCoinCore
{
    public abstract class IDetector
    {
        public DetectionAgent[] agent;
        //public CloudCoin returnCoin;
        public Response[] responseArray = new Response[25];
        private int[] working_triad = { 0, 1, 2 };//place holder
        public bool[] raidaIsDetecting = { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
        public string[] lastDetectStatus = { "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected", "notdetected" };
        public string[] echoStatus = { "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply", "noreply" };

        /*  INSTANCE VARIABLES */
        public RAIDA raida;
            public IFileUtils fileUtils;
            public int detectTime = 5000;

            /*  CONSTRUCTOR */
            public IDetector(IFileUtils fileUtils, int timeout)
            {

                this.raida = new RAIDA(timeout);
                this.fileUtils = fileUtils;
            }// end Detect constructor
        public abstract void detectOne(int raida_id, int nn, int sn, String an, String pan, int d);
        public abstract int[] detectAll();
        public abstract int[] partialDetectAll();

        public abstract CoinUtils partialDetectCoin(CoinUtils cu, int milliSecondsToTimeOut);
       

    }
}
