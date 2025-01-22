using Shared.Enums;
using SharedEnums.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Models
{
    public class Trade
    {
        public Trade()
        {
            CreatedAt = DateTime.Now;
        }
        public int Id { get; set; }
        public string? Symbol { get; set; }

        [DefaultValue(0)]
        public double? TriggerPrice { get; set; }

        [DefaultValue(0)]
        public double? EntryPrice { get; set; }

        [DefaultValue(0)]
        public double? StopPrice { get; set; }

        [DefaultValue(0)]
        public double? ExitPrice { get; set; }

        public List<double>? Targets { get; set; }

        public double? PnL { get; set; }

        public bool IsLoss { get; set; }

        [DefaultValue(0)]
        public double? Fee { get; set; }

        public Status Status { get; set; }

        public SideType SideType { get; set; }

        public OrderType OrderType { get; set; }

        public TradeRating TradeRating { get; set; }

        public TradeType TradeType { get; set; }

        public List<string>? ScreenshotsUrls { get; set; }

        [DefaultValue(0)]
        public int? TradeDurationInCandles { get; set; }
        
        /// <summary>
        ///  The research type can be determined based on the trade type.
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


        public DateTime CreatedAt { get; set; }

    }
}
