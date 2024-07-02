using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using SharedEnums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class NewTradeVM
    {
        public NewTradeVM()
        {
            CurrentTrade = new ResearchFirstBarPullbackDisplay();

            YesNoOptions = new List<SelectListItem> 
            {
                new SelectListItem { Value = "1", Text = "Yes" },
                new SelectListItem { Value = "0", Text = "No" }
            };
        }
        public TimeFrame TimeFrame { get; set; }

        public Strategy Strategy { get; set; }

        public TradeType TradeType { get; set; }

        public SideType Side { get; set; }

        public ResearchFirstBarPullbackDisplay CurrentTrade { get; set; }

        public List<SelectListItem> YesNoOptions { get; set; }
    }
}
