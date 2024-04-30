﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Journal
    {
        [Key]
        public int Id { get; set; }
        public string? Pre { get; set; }

        public string? During { get; set; }

        public string? Exit { get; set; }

        public string? Post { get; set; }

        public int PaperTradeId { get; set; }
        [ForeignKey(nameof(PaperTradeId))]

        public PaperTrade? PaperTrade { get; set; }
    }
}
