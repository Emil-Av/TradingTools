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
    public class PartialViewsVM
    {

        public PartialViewsVM()
        {
            ResearchFirstBarPullbackDisplay = new();
            TradesVM = new();
            ResearchCradle = new();
            TradeRating = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "A+"},
                new SelectListItem { Value = "1", Text = "A"},
                new SelectListItem { Value = "2", Text = "A-"},
                new SelectListItem { Value = "3", Text = "Book of Horror"}
            };

            YesNoOptions = new List<SelectListItem>
            {
                new SelectListItem {Value = "0", Text = "No"},
                new SelectListItem {Value = "1", Text = "Yes"}
            };



            OrderType = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Market"},
                new SelectListItem { Value = "1", Text = "Limit"},
            };
        }
        public ResearchFirstBarPullbackDisplay ResearchFirstBarPullbackDisplay { get; set; }

        public TradesVM TradesVM { get; set; }

        public ResearchCradle ResearchCradle { get; set; }

        public List<SelectListItem> YesNoOptions { get; set; }
        public List<SelectListItem> OrderType { get; set; }
        public List<SelectListItem> TradeRating { get; set; }
    }
}
