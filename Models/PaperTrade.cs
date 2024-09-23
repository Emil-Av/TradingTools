using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaperTrade : Trade
    {
        public int JournalId { get; set; }

        [ForeignKey(nameof(JournalId))]
        public Journal? Journal { get; set; }

        public int ReviewId { get; set; }

        [ForeignKey(nameof(ReviewId))]
        public Review? Review { get; set; }
    }
}
