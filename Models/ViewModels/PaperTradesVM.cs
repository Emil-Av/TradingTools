using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class PaperTradesVM
    {
        // The trade being displayed
        public PaperTrade CurrentTrade { get; set; }

        // The journal for the CurrentTrade

        public Journal Journal { get; set; }

        public Review Review { get; set; }

        // The number of sample sizes for a strategy and time frame
        public int NumberSampleSizes { get; set; }

        // The current number of trades for the latest sample size
        public int TradesInSampleSize { get; set; }
    }
}
