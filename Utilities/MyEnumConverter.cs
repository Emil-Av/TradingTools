using Utilities.Enums;

namespace Utilities
{
    public class MyEnumConverter
    {
        public static TradeType SetTradeType(string tradeType)
        {
            Dictionary<string, TradeType> tradeTypes = new Dictionary<string, TradeType>()
            {
                { "Trade", TradeType.Trade },
                { "PaperTrade" , TradeType.PaperTrade }
            };

            return tradeTypes[tradeType];
        }

        public static TimeFrame SetTimeFrameFromString(string tf)
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

            return timeFrames[tf];
        }

        public static string SetTimeFrameFromEnum(TimeFrame? tf)
        {
            Dictionary<TimeFrame?, string> timeFrames = new Dictionary<TimeFrame?, string>()
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

        public static Strategy SetStrategyFromString(string strategy)
        {
            Dictionary<string, Strategy> strategies = new Dictionary<string, Strategy>()
            {
                { "Cradle", Strategy.Cradle },
                { "First Bar Below-Above", Strategy.FirstBarBelowAbove }
            };

            return strategies[strategy];
        }

        public static string SetStrategyFromEnum(Strategy? strategy)
        {
            Dictionary<Strategy?, string> strategies = new Dictionary<Strategy?, string>()
            {
                { Strategy.Cradle, "Cradle" },
                { Strategy.FirstBarBelowAbove, "First Bar Below-Above" }
            };

            return strategies[strategy];
        }
    }
}
