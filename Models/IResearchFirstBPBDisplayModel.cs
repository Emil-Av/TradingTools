using Microsoft.AspNetCore.Mvc.Rendering;
using Models.ViewModels.DisplayClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IResearchFirstBPBDisplayModel
    {
        ResearchFirstBarPullbackDisplay ResearchFirstBarPullbackDisplay { get; set; }

        List<SelectListItem> YesNoOptions { get; set; }

        List<SelectListItem> TradeRating { get; set; }
    }
}
