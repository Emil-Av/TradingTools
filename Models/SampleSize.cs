using SharedEnums.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class SampleSize
    {
        public int Id { get; set; }

        public Strategy Strategy { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public TradeType TradeType { get; set; }

        public int? ReviewId { get; set; }

        [ForeignKey(nameof(ReviewId))]
        public Review Review { get; set; }
    }
}
