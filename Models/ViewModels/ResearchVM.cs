using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
//using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    // Need to figure if the logic SelectListItem should be in the controller or in the Model layer. Then need to figure out how to render <options selected> using SelectListItem.
    public class ResearchVM
    {
        public ResearchVM()
        {
            AvailableStrategies = new List<Strategy>();
            AvailableTimeframes = new List<TimeFrame>();
            YesNoOptions = new List<SelectListItem>
            {
                new SelectListItem {Value = "Yes", Text = "Yes"},
                new SelectListItem {Value = "No", Text = "No"}
            };
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

        public List<SelectListItem> YesNoOptions { get; set; }
    }
}
