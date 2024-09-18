using SharedEnums.Enums;

namespace Shared
{
    public class MyEnumConverter
    {
        public static string OrderTypeFromEnum(OrderType orderType)
        {
            Dictionary<OrderType, string> statusType = new Dictionary<OrderType, string>()
            {
                { OrderType.Market, "Market" },
                { OrderType.Limit, "Limit" },
                { OrderType.StopLoss, "Stop Loss" }
            };

            return statusType[orderType];
        }

        public static Result<OrderType> OrderTypeFromString(string orderType)
        {
            Dictionary<string, OrderType> orderTypes = new Dictionary<string, OrderType>()
            {
                { "Market", OrderType.Market },
                { "Limit", OrderType.Limit },
                { "Stop Loss", OrderType.StopLoss }
            };
            try
            {
                return Result<OrderType>.SuccessResult(orderTypes[orderType]);
            }
            catch
            {
                return Result<OrderType>.ErrorResult($"Error converting order type from a string. Value given: {orderType}");
            }
        }

        public static Result<Status> StatusFromString(string status)
        {
            Dictionary<string, Status> statusTypes = new Dictionary<string, Status>()
            {
                { "Pending", Status.Pending },
                { "Opened", Status.Opened },
                { "Closed", Status.Closed }
            };

            try
            {
                return Result<Status>.SuccessResult(statusTypes[status]);
            }
            catch
            {
                return Result<Status>.ErrorResult($"Error converting status from a string. Value given: {status}");
            }
        }

        public static Result<SideType> SideTypeFromString(string sideType)
        {
            Dictionary<string, SideType> sideTypes = new Dictionary<string, SideType>() 
            {
                { "Long", SideType.Long },
                { "Short", SideType.Short }
            };

            try
            {
                return Result<SideType>.SuccessResult(sideTypes[sideType]);
            }
            catch
            {
                return Result<SideType>.ErrorResult($"Error converting side type from a string. Value given: {sideType}");
            }
        }

        public static Result<TradeType> TradeTypeFromString(string tradeType)
        {
            Dictionary<string, TradeType> tradeTypes = new Dictionary<string, TradeType>()
            {
                { "Trade", TradeType.Trade },
                { "PaperTrade" , TradeType.PaperTrade },
                { "Research", TradeType.Research },
            };

            try
            {
                return Result<TradeType>.SuccessResult(tradeTypes[tradeType]);
            }
            catch
            {
                return Result<TradeType>.ErrorResult($"Error converting the trade type from a string. Value given: {tradeType}");
            }
        }

        public static string TradeTypeFromEnum(TradeType tradeType)
        {
            Dictionary<TradeType, string> tradeTypes = new Dictionary<TradeType, string>()
            {
                { TradeType.Trade , "Trade"},
                { TradeType.PaperTrade, "PaperTrade" },
                { TradeType.Research, "Research" },
            };

            return tradeTypes[tradeType];
        }

        public static Result<TimeFrame> TimeFrameFromString(string timeFrame)
        {

            Dictionary<string, TimeFrame> timeFrames = new Dictionary<string, TimeFrame>()
            {
                { "5M", TimeFrame.M5 },
                { "10M", TimeFrame.M10 },
                { "15M", TimeFrame.M15 },
                { "30M", TimeFrame.M30 },
                { "1H", TimeFrame.H1 },
                { "2H", TimeFrame.H2 },
                { "4H", TimeFrame.H4 },
                { "D", TimeFrame.D }

            };

            try
            {
                return Result<TimeFrame>.SuccessResult(timeFrames[timeFrame]);
            }
            catch
            {
                return Result<TimeFrame>.ErrorResult($"Error converting the time frame from as string. Value given: {timeFrame}");
            }
        }

        public static string TimeFrameFromEnum(TimeFrame timeFrame)
        {
            Dictionary<TimeFrame, string> timeFrames = new Dictionary<TimeFrame, string>()
            {
                { TimeFrame.M5, "5M" },
                { TimeFrame.M10 , "10M" },
                { TimeFrame.M15 , "15M" },
                { TimeFrame.M30 , "30M" },
                { TimeFrame.H1 , "1H" },
                { TimeFrame.H2 , "2H" },
                { TimeFrame.H4 , "4H" },
                { TimeFrame.D , "D" }

            };

            return timeFrames[timeFrame];
        }

        public static Result<Strategy> StrategyFromString(string strategy)
        {
            Dictionary<string, Strategy> strategies = new Dictionary<string, Strategy>()
            {
                { "Cradle", Strategy.Cradle },
                { "First Bar Pullback", Strategy.FirstBarPullback }
            };

            try
            {
                return Result<Strategy>.SuccessResult(strategies[strategy]);
            }
            catch
            {
                return Result<Strategy>.ErrorResult($"Error converting the strategy from a string. Value given: {strategy}");
            }
        }

        public static string StrategyFromEnum(Strategy? strategy)
        {
            Dictionary<Strategy?, string> strategies = new Dictionary<Strategy?, string>()
            {
                { Strategy.Cradle, "Cradle" },
                { Strategy.FirstBarPullback, "First Bar Pullback" }
            };

            return strategies[strategy];
        }
    }
}
