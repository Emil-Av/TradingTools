using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedEnums.Enums;
using Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using Microsoft.AspNetCore.Mvc.Rendering;

namespace Models.ViewModels
{
    public class ResearchVM
    {
        #region Constructor
        public ResearchVM()
        {
            AvailableStrategies = new List<Strategy>();
            AvailableTimeframes = new List<TimeFrame>();
            YesNoOptions = new List<SelectListItem>
            {
                new SelectListItem {Value = "1", Text = "Yes"},
                new SelectListItem {Value = "0", Text = "No"}
            };
        }

        #endregion

        #region Properties
        public TimeFrame CurrentTimeFrame { get; set; }

        public Strategy CurrentStrategy { get; set; }

        public SampleSize CurrentSampleSize { get; set; }

        public int CurrentSampleSizeId { get; set; }

        public ResearchFirstBarPullbackDisplay CurrentTrade { get; set; }

        public List<ResearchFirstBarPullbackDisplay> AllTrades { get; set; }

        public List<SelectListItem> YesNoOptions { get; set; }

        public List<Strategy> AvailableStrategies { get; set; }

        public List<TimeFrame> AvailableTimeframes { get; set; }

        // The number of sample sizes for a strategy and time frame
        public int NumberSampleSizes { get; set; }

        // The current number of trades for the latest sample size
        public int TradesInSampleSize { get; set; }

        #endregion

        #region Methods

        public string SetSampleSizeParams(string timeFrame, string strategy, string sampleSizeNumber)
        {
            List<string> errors = new List<string>();
            string error = string.Empty;
            ToEnumFromStringResult<TimeFrame> timeFrameResult = MyEnumConverter.TimeFrameFromString(timeFrame);
            ToEnumFromStringResult<Strategy> strategyResult = MyEnumConverter.StrategyFromString(strategy);
            if (!timeFrameResult.Success)
            {
                errors.Add(timeFrameResult.ErrorMessage);
            }
            if (!strategyResult.Success)
            {
                errors.Add(strategyResult.ErrorMessage);
            }
            if (!Int32.TryParse(sampleSizeNumber, out int _sampleSizeNumber))
            {
                errors.Add("Error parsing the sample size number");
            }
            else
            {
                CurrentSampleSizeId = _sampleSizeNumber;
            }

            if (errors.Any())
            {
                error = string.Join("<br>", errors);
            }

            CurrentTimeFrame = timeFrameResult.Value;
            CurrentStrategy = strategyResult.Value;

            return error;
        }

        #endregion

    }
}
