using Shared.Enums;
using SharedEnums.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.ViewModels.DisplayClasses
{
    public class TradeDisplay
    {
        public string? IdDisplay { get; set; }
        public string? SymbolDisplay { get; set; }

        public string? TriggerPriceDisplay { get; set; }

        public string? EntryPriceDisplay { get; set; }

        public string? StopPriceDisplay { get; set; }

        public string? ExitPriceDisplay { get; set; }

        public string? TargetsDisplay { get; set; }

        public string? PnLDisplay { get; set; }

        public string? FeeDisplay { get; set; }

        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public Status StatusDisplay { get; set; }

        public SideType? SideTypeDisplay { get; set; }

        public OrderType? OrderTypeDisplay { get; set; }

        public TradeRating? TradeRatingDisplay { get; set; }

        public List<string>? ScreenshotsUrlsDisplay { get; set; }

        public int? SampleSizeId { get; set; }
        [ForeignKey(nameof(SampleSizeId))]

        public SampleSize? SampleSize { get; set; }

        public string? TradeDurationInCandlesDisplay { get; set; }
    }
}
