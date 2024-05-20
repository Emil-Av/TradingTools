using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SampleSize
    {
        public int Id { get; set; }

        public Strategy Strategy { get; set; }

        public TimeFrame TimeFrame { get; set; }
    }
}
