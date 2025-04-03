using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class PartialViewsVM
    {

        public PartialViewsVM()
        {
            ResearchFirstBarPullbackDisplay = new();
            TradesVM = new();
            ResearchCradle = new();
        }
        public ResearchFirstBarPullbackDisplay ResearchFirstBarPullbackDisplay { get; set; }

        public TradesVM TradesVM { get; set; }

        public ResearchCradle ResearchCradle { get; set; }

        public List<SelectListItem> YesNoOptions { get; set; }

        public List<SelectListItem> TradeRating { get; set; }
    }
}
