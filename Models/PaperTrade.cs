using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaperTrade : Trade
    {
        public PaperTrade()
        {
            ScreenshotsUrls = new List<string>();
        }
        public int Id { get; set; }
    }
}
