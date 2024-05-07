using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Trade
    {
        public string? Symbol { get; set; }

        public double? TriggerPrice { get; set; }

        public double? EntryPrice { get; set; }

        public double? StopPrice { get; set; }

        public double? FirstTarget { get; set; }

        public double? SecondTarget { get; set; }

        public double? ExitPrice { get; set; }

        public double? Profit { get; set; }

        public double? Loss { get; set; }

        public double? Fee { get; set; }

        public SampleSize? SampleSize { get; set; }

        public TimeFrame? TimeFrame { get; set; }

        public Status? Status { get; set; }

        public Strategy? Strategy { get; set; }

        public SideType? SideType { get; set; }

        public OrderType? OrderType { get; set; }

        public List<string>? ScreenshotsUrls { get; set; }

        public DateTime? EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public DateTime? TradeDuration { get; set; }
    }

    public enum TimeFrame { M5, M10, M15, M30, H1, H2, H4, D }

    public enum Status { Active, Closed, Pending, Cancelled }

    public enum Strategy { FirstBarBelowAbove, Cradle }

    public enum SideType { Long, Short }

    public enum OrderType { Limit, Market }

    public enum TradeType { Trade, PaperTrade }
}
