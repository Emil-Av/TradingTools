using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using Newtonsoft.Json;
using Shared;
using SharedEnums.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class NewTradeVM
    {
        public NewTradeVM()
        {
            YesNoOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Yes" },
                new SelectListItem { Value = "0", Text = "No" }
            };
            ResearchData = new();
            ResearchFirstBarPullbackDisplay = new();
        }

        #region Properties
        public TimeFrame TimeFrame { get; set; }

        public Strategy Strategy { get; set; }

        public TradeType TradeType { get; set; }

        public SideType Side { get; set; }

        public object ResearchData { get; set; }

        public ResearchFirstBarPullbackDisplay ResearchFirstBarPullbackDisplay { get; set; }

        public List<SelectListItem> YesNoOptions { get; set; }

        #endregion

        #region Method

        public string SetValues(string tradeParams, string tradeData)
        {
            string error = string.Empty;
            try
            {
                Dictionary<string, string> tradeDataObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(tradeParams);
                TimeFrame = MyEnumConverter.TimeFrameFromString(tradeDataObject["timeFrame"]);
                Strategy = MyEnumConverter.StrategyFromString(tradeDataObject["strategy"]);
                TradeType = MyEnumConverter.TradeTypeFromString(tradeDataObject["tradeType"]);
                Side = MyEnumConverter.SideTypeFromString(tradeDataObject["tradeSide"]);

                if (TradeType == TradeType.Research && Strategy == Strategy.FirstBarBelowAbove)
                {
                    ResearchData = JsonConvert.DeserializeObject<ResearchFirstBarPullbackDisplay>(tradeData);
                }
            }
            catch (Exception ex)
            {
                error = $"Error in NewTradeVM.SetValues(): {ex.Message}";
            }

            return error;
        }

        #endregion
    }
}
