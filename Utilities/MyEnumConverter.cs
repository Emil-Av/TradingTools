using Models;

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

        public static TimeFrame SetTimeFrame(string tf)
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

        public static Strategy SetStrategy(string strategy)
        {
            Dictionary<string, Strategy> strategies = new Dictionary<string, Strategy>()
            {
                { "Cradle", Strategy.Cradle },
                { "First Bar Below-Above", Strategy.FirstBarBelowAbove }
            };

            return strategies[strategy];
        }
    }
}
