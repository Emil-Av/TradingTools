using Shared;
using SharedEnums.Enums;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LoadTradeParams
    {
        public void ConvertValuesFromView()
        {
            Result<Status> resultStatus = MyEnumConverter.StatusFromString(StatusFromView);
            Result<TimeFrame> resultTimeFrame = MyEnumConverter.TimeFrameFromString(TimeFrameFromView);
            Result<Strategy> resultStrategy = MyEnumConverter.StrategyFromString(StrategyFromView);
            Result<TradeType> resultTradeType = MyEnumConverter.TradeTypeFromString(TradeTypeFromView);
            Result<SideType> resultSideType = MyEnumConverter.SideTypeFromString(TradeTypeFromView);
            Result<OrderType> resultOrderType = MyEnumConverter.OrderTypeFromString(OrderTypeFromView);

            if (resultStatus.Success)
            {
                Status = resultStatus.Value;
            }
            if (resultTimeFrame.Success)
            {
                TimeFrame = resultTimeFrame.Value;
            }
            if (resultStrategy.Success)
            {
                Strategy = resultStrategy.Value;
            }
            if (resultTradeType.Success)
            {
                TradeType = resultTradeType.Value;
            }
            if (resultSideType.Success)
            {
                SideType = resultSideType.Value;
            }
            if (resultOrderType.Success)
            {
                OrderType = resultOrderType.Value;
            }

            SampleSizeNumber = int.Parse(SampleSizeNumberFromView);
            TradeNumber = int.Parse(TradeNumberFromView);
            ShowLastTrade = bool.Parse(ShowLastTradeFromView);
            SampleSizeChanged = bool.Parse(SampleSizeChangedFromView);
        }

        #region Values from the view
        public string StatusFromView { get; set; }

        public string TimeFrameFromView { get; set; }

        public string StrategyFromView { get; set; }

        public string TradeTypeFromView { get; set; }

        public string SideTypeFromView { get; set; }

        public string OrderTypeFromView { get; set; }

        public string SampleSizeNumberFromView { get; set; }

        public string TradeNumberFromView { get; set; }

        public string ShowLastTradeFromView { get; set; }

        public string SampleSizeChangedFromView { get; set; }

        #endregion

        #region Properties for after conversion

        public bool SampleSizeChanged { get; set; }

        public bool ShowLastTrade { get; set; }

        public int TradeNumber { get; set; }

        public int SampleSizeNumber { get; set; }

        public Status Status { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public Strategy Strategy { get; set; }

        public TradeType TradeType { get; set; }

        public SideType SideType { get; set; }

        public OrderType OrderType { get; set; }

        #endregion
    }
}
