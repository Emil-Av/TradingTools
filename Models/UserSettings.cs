using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserSettings
    {
        public int Id { get; set; }

        /// <summary>
        ///  Last used time frame in PaperTrades
        /// </summary>

        public TimeFrame PTTimeFrame { get; set; }

        [DefaultValue(0)]
        /// <summary>
        ///  Last used strategy in PaperTrades
        /// </summary>
        public Strategy PTStrategy { get; set; }    
    }
}
