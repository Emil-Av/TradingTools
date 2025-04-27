using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using SharedEnums.Enums;
using Shared;

namespace Models.ViewModels
{
    public class ResearchVM : PartialViewsVM, IResearchDisplayModel
    {
        #region Constructor
        public ResearchVM()
        {
            AvailableStrategies = new List<EStrategy>();
            AvailableTimeframes = new List<ETimeFrame>();
            CurrentTrade = new();
            TradeData = new();

            YesNoOptions = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "No"},
                new SelectListItem {Value = "1", Text = "Yes"}
            };

            TradeRating = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "A+"},
                new SelectListItem { Value = "1", Text = "A"},
                new SelectListItem { Value = "2", Text = "A-"},
                new SelectListItem { Value = "3", Text = "Book of Horror"}
            };

            OrderType = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Market"},
                new SelectListItem { Value = "1", Text = "Limit"},
            };
        }

        #endregion

        #region Public Properties

        public ETimeFrame CurrentTimeFrame { get; set; }

        public EStrategy CurrentStrategy { get; set; }

        public EStatus Status { get; set; }

        public SampleSize CurrentSampleSize { get; set; }

        public int CurrentSampleSizeId { get; set; }

        // The selected sample size for the selected time frame and strategy (e.g. 2nd out of 5)
        public int CurrentSampleSizeNumber { get; set; }

        public ResearchFirstBarPullbackDisplay ResearchFirstBarPullbackDisplay { get; set; }

        public object CurrentTrade { get; set; }

        public ResearchCradle ResearchCradle { get; set; }

        public TradeDisplay TradeData { get; set; }

        public List<object> AllTrades { get; set; }

        public List<SelectListItem> YesNoOptions { get; set; }

        public List<SelectListItem> TradeRating { get; set; }

        public List<SelectListItem> OrderType { get; set; }

        public List<EStrategy> AvailableStrategies { get; set; }

        public List<ETimeFrame> AvailableTimeframes { get; set; }

        // The number of sample sizes for a strategy and time frame
        public int NumberSampleSizes { get; set; }

        // The current number of trades for the latest sample size
        public int TradesInSampleSize { get; set; }

        public bool HasSampleSizeChanged { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///  Sets the paramaters for the sample size that has to be displayed (time frame, strategy)
        /// </summary>
        /// <param name="timeFrame"></param>
        /// <param name="strategy"></param>
        /// <param name="sampleSizeNumber"></param>
        /// <param name="isSampleSizeChanged"></param>
        /// <returns></returns>
        public string SetSampleSizeParams(string timeFrame, string strategy, string sampleSizeNumber, string isSampleSizeChanged)
        {
            List<string> errors = new List<string>();
            string error = string.Empty;
            Result<ETimeFrame> timeFrameResult = MyEnumConverter.TimeFrameFromString(timeFrame);
            Result<EStrategy> strategyResult = MyEnumConverter.StrategyFromString(strategy);
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
            if (bool.TryParse(isSampleSizeChanged, out bool tempIsSampleSizeChanged))
            {
                HasSampleSizeChanged = tempIsSampleSizeChanged;
            }
            else
            {
                errors.Add("Error parsing isSampleSizeChanged variable");
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
