using Shared.Enums;
using SharedEnums.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Trade
    {
        public string? Symbol { get; set; }

        public double? TriggerPrice { get; set; }

        public double? EntryPrice { get; set; }

        public double? StopPrice { get; set; }

        public List<double>? Targets { get; set; }

        public double? ExitPrice { get; set; }

        public double? Profit { get; set; }

        public double? Loss { get; set; }

        public double? Fee { get; set; }

        public TimeFrame? TimeFrame { get; set; }

        public Status? Status { get; set; }

        public Strategy? Strategy { get; set; }

        public SideType? SideType { get; set; }

        public OrderType? OrderType { get; set; }

        public TradeRating? TradeRating { get; set; }

        public List<string>? ScreenshotsUrls { get; set; }

        public int SampleSizeId { get; set; }
        [ForeignKey(nameof(SampleSizeId))]

        public SampleSize? SampleSize { get; set; }

        public DateTime? EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public DateTime? TradeDuration { get; set; }
    }
}
