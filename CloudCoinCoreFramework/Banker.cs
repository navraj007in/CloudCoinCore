using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinCore
{
    public abstract class IBanker
    {
        public IFileUtils fileUtils;

    public IBanker(IFileUtils fileUtils)
    {
        this.fileUtils = fileUtils;
    }

        public abstract int[] countCoins(String directoryPath);
}
}
