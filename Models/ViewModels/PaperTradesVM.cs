using Models.ViewModels.DisplayClasses;
using SharedEnums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class PaperTradesVM
    {
        public PaperTradesVM()
        {
            AvailableStrategies = new List<Strategy>();
            AvailableTimeframes = new List<TimeFrame>();
            TradeData = new();
        }
        public int TradesInSampleSize { get; set; }

        // The number of sample sizes for a strategy and time frame
        public int NumberSampleSizes { get; set; }

        public int CurrentSampleSizeNumber { get; set; }

        // The trade being displayed
        public PaperTrade CurrentTrade { get; set; }

        public TradeDisplay TradeData { get; set; }

        public SampleSize CurrentSampleSize { get; set; }

        // The current number of trades for the latest sample size
        public List<Strategy> AvailableStrategies { get; set; }

        public List<TimeFrame> AvailableTimeframes { get; set; }
    }
}
