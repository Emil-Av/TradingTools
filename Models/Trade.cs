using Shared.Enums;
using SharedEnums.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Models
{
    public class Trade
    {
        public int Id { get; set; }
        public string? Symbol { get; set; }

        public double? TriggerPrice { get; set; }

        public double? EntryPrice { get; set; }

        public double? StopPrice { get; set; }

        public double? ExitPrice { get; set; }

        public List<double>? Targets { get; set; }

        public double? PnL { get; set; }

        public bool IsLoss { get; set; }

        public double? Fee { get; set; }

        public Status Status { get; set; }

        public SideType SideType { get; set; }

        public OrderType OrderType { get; set; }

        public TradeRating TradeRating { get; set; }

        public List<string>? ScreenshotsUrls { get; set; }

        public int? TradeDurationInCandles { get; set; }
        
        /// <summary>
        ///  Based on the trade type, the research type can be determined.
        /// </summary>
        public int? ResearchId { get; set; }

        public int SampleSizeId { get; set; }

        [ForeignKey(nameof(SampleSizeId))]
        public SampleSize? SampleSize { get; set; }

        /// <summary>
        ///  All trades except research trades will have a Journal.
        /// </summary>
        public int? JournalId { get; set; }

        [ForeignKey(nameof(JournalId))]
        public Journal? Journal { get; set; }

    }
}
