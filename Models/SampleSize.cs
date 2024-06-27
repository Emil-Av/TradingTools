using SharedEnums.Enums;

namespace Models
{
    public class SampleSize
    {
        public int Id { get; set; }

        public Strategy Strategy { get; set; }

        public TimeFrame TimeFrame { get; set; }

        public TradeType TradeType { get; set; }
    }
}
