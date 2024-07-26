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

        public TradeType Type { get; set; }

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
                Result<TimeFrame> timeFrameResult = MyEnumConverter.TimeFrameFromString(tradeDataObject["timeFrame"]);
                Result<Strategy> strategyResult = MyEnumConverter.StrategyFromString(tradeDataObject["strategy"]);
                Result<TradeType> typeResult = MyEnumConverter.TradeTypeFromString(tradeDataObject["tradeType"]);
                Result<SideType> sideResult = MyEnumConverter.SideTypeFromString(tradeDataObject["tradeSide"]);

                List<string> errors = new List<string>();

                if (!timeFrameResult.Success)
                {
                    errors.Add(timeFrameResult.ErrorMessage);
                }
                if (!strategyResult.Success)
                {
                    errors.Add(strategyResult.ErrorMessage);    
                }
                if (!typeResult.Success)
                {
                    errors.Add(typeResult.ErrorMessage);
                }
                if (!sideResult.Success)
                {
                    errors.Add(sideResult.ErrorMessage);
                }

                if (errors.Any())
                {
                    return error = string.Join("<br>", errors);
                }

                TimeFrame = timeFrameResult.Value;
                Strategy = strategyResult.Value;
                Type = typeResult.Value;
                Side = sideResult.Value;

                if (Type == TradeType.Research && Strategy == Strategy.FirstBarBelowAbove)
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
