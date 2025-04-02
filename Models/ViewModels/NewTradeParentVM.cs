using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class NewTradeParentVM
    {

        public NewTradeParentVM()
        {
            NewTradeVM = new();
            TradesVM = new();
        }
        public NewTradeVM NewTradeVM { get; set; }

        public TradesVM TradesVM { get; set; }
    }
}
