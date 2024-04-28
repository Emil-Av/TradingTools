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
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Instrument { get; set; }


        public double? TriggerPrice { get; set; }

        [Required]
        public double? EntryPrice { get; set; }

        [Required]
        public double? StopPrice { get; set; }

        [Required]

        public double? FirstTarget { get; set; }

        public double? SecondTarget { get; set; }

        public double? ExitPrice { get; set; }

        public double? Profit { get; set; }

        public double? Loss { get; set; }

        public double? Fee { get; set; }

        public enum TimeFrame { M5, M10, M15, M30, H1, H2, H4, D}

        public enum Status { Active, Closed, Pending, Cancelled }

        public enum Strategy { FirstBarBelowAbove, Cradle }

        public enum SideType { Long, Short }

        public enum OrderType { Limit, Market}

        public DateTime? EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }

        public DateTime? TradeDuration { get; set; }
    }
}
