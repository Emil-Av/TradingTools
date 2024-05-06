using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    /// <summary>
    ///  One review object per sample size of 20 trades.
    /// </summary>
    public class Review
    {
        public int Id { get; set; }
        public string? SampleSizeReview { get; set; }
        public Strategy? Strategy { get; set; }
        public TimeFrame? TimeFrame { get; set; }
        public TradeType TradeType { get; set; }

    }
}
