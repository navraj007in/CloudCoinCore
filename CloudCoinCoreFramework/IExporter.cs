using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinCore
{
    public abstract class IExporter
    {
        IFileUtils fileUtils;

        /* CONSTRUCTOR */
        public IExporter(IFileUtils fileUtils)
        {
            this.fileUtils = fileUtils;
        }

        /* PUBLIC METHODS */
        public abstract void writeJPEGFiles(int m1, int m5, int m25, int m100, int m250, String tag);

        /* Write JSON to .stack File  */
        public abstract bool writeJSONFile(int m1, int m5, int m25, int m100, int m250, String tag);


        /* PRIVATE METHODS */
        protected abstract void jpegWriteOne(String path, String tag, String bankFileName, String frackedFileName);
    }
}
