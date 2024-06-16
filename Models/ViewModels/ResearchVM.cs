using Models.ViewModels.DisplayClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Models.ViewModels
{
    public class ResearchVM
    {
        public ResearchVM()
        {
            AvailableStrategies = new List<Strategy>();
            AvailableTimeframes = new List<TimeFrame>();
        }
        public ResearchFirstBarPullbackDisplay CurrentTrade { get; set; }

        public List<ResearchFirstBarPullbackDisplay> AllTrades { get; set; }   

        public SampleSize CurrentSampleSize { get; set; }

        public List<Strategy> AvailableStrategies { get; set; }

        public List<TimeFrame> AvailableTimeframes { get; set; }

        // The number of sample sizes for a strategy and time frame
        public int NumberSampleSizes { get; set; }

        // The current number of trades for the latest sample size
        public int TradesInSampleSize { get; set; }
    }
}
