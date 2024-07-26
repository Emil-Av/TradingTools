using SharedEnums.Enums;

namespace Shared
{
    public class MyEnumConverter
    {
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
            catch (Exception ex) 
            {
                return Result<SideType>.ErrorResult("Side not selected.");
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
            catch (Exception ex)
            {
                return Result<TradeType>.ErrorResult("Type not selected.");
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

        public static Result<TimeFrame> TimeFrameFromString(string tf)
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
                return Result<TimeFrame>.SuccessResult(timeFrames[tf]);
            }
            catch (Exception ex)
            {
                return Result<TimeFrame>.ErrorResult("Time frame not selected.");
            }
        }

        public static string TimeFrameFromEnum(TimeFrame tf)
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

            return timeFrames[tf];
        }

        public static Result<Strategy> StrategyFromString(string strategy)
        {
            Dictionary<string, Strategy> strategies = new Dictionary<string, Strategy>()
            {
                { "Cradle", Strategy.Cradle },
                { "First Bar Pullback", Strategy.FirstBarBelowAbove }
            };

            try
            {
                return Result<Strategy>.SuccessResult(strategies[strategy]);
            }
            catch (Exception ex)
            {
                return Result<Strategy>.ErrorResult("Strategy not selected.");
            }
        }

        public static string StrategyFromEnum(Strategy? strategy)
        {
            Dictionary<Strategy?, string> strategies = new Dictionary<Strategy?, string>()
            {
                { Strategy.Cradle, "Cradle" },
                { Strategy.FirstBarBelowAbove, "First Bar Pullback" }
            };

            return strategies[strategy];
        }
    }
}
