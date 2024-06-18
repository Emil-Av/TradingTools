using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Models.ViewModels.DisplayClasses
{
    public class ResearchFirstBarPullbackDisplay
    {
        /// <summary>
        ///  Prepares the DB values for the view.
        /// </summary>
        /// <param name="dbResearchObj"></param>
        public ResearchFirstBarPullbackDisplay(ResearchFirstBarPullback dbResearchObj)
        {
            ScreenshotsUrls = dbResearchObj.ScreenshotsUrls;
            SideTypeDisplay = dbResearchObj.SideType;
            OneToOneHitOnDisplay = dbResearchObj.OneToOneHitOn < 5 ? dbResearchObj.OneToOneHitOn.ToString() : ">5";
            IsOneToThreeHitDisplay = dbResearchObj.IsOneToThreeHit ? "Yes" : "No";
            IsOneToFiveHitDisplay = dbResearchObj.IsOneToFiveHit ? "Yes" : "No";
            IsBreakevenDisplay = dbResearchObj.IsBreakeven ? "Yes" : "No";
            IsLossDisplay = dbResearchObj.IsLoss ? "Yes" : "No";
            MaxRRDisplay = dbResearchObj.MaxRR;
            MarketGaveSmthDisplay = dbResearchObj.MarketGaveSmth ? "Yes" : "No";
            IsEntryAfter3To5BarsDisplay = dbResearchObj.IsEntryAfter3To5Bars ? "Yes" : "No";
            IsEntryAfter5BarsDisplay = dbResearchObj.IsEntryAfter5Bars ? "Yes" : "No";
            IsEntryAtPreviousSwingOnTriggerDisplay = dbResearchObj.IsEntryAtPreviousSwingOnTrigger ? "Yes" : "No";
            IsEntryBeforePreviousSwingOnTriggerDisplay = dbResearchObj.IsEntryBeforePreviousSwingOnTrigger ? "Yes" : "No";
            IsEntryBeforePreviousSwingOn4HDisplay = dbResearchObj.IsEntryBeforePreviousSwingOn4H ? "Yes" : "No";
            IsEntryBeforePreviousSwingOnDDisplay = dbResearchObj.IsEntryBeforePreviousSwingOnD ? "Yes" : "No";
            IsMomentumTradeDisplay = dbResearchObj.IsMomentumTrade ? "Yes" : "No";
            IsTrendTradeDisplay = dbResearchObj.IsTrendTrade ? "Yes" : "No";
            IsTriggerTrendingDisplay = dbResearchObj.IsTriggerTrending ? "Yes" : "No";
            Is4HTrendingDisplay = dbResearchObj.Is4HTrending ? "Yes" : "No";
            IsDTrendingDisplay = dbResearchObj.IsDTrending ? "Yes" : "No";
            IsEntryAfteriBarDisplay = dbResearchObj.IsEntryAfteriBar ? "Yes" : "No";
            IsSignalBarStrongReversalDisplay = dbResearchObj.IsSignalBarStrongReversal ? "Yes" : "No";
            IsSignalBarInTradeDirectionDisplay = dbResearchObj.IsSignalBarInTradeDirection ? "Yes" : "No";
            FullATROneToOneHitOnDisplay = dbResearchObj.FullATROneToOneHitOn;
            IsFullATROneToThreeHitDisplay = dbResearchObj.IsFullATROneToThreeHit ? "Yes" : "No";
            IsFullATROneToFiveHitDisplay = dbResearchObj.IsFullATROneToFiveHit ? "Yes" : "No";
            IsFullATRBreakevenDisplay = dbResearchObj.IsFullATRBreakeven ? "Yes" : "No";
            IsFullATRLossDisplay = dbResearchObj.IsFullATRLoss ? "Yes" : "No";
            FullATRMaxRRDisplay = dbResearchObj.FullATRMaxRR;
            FullATRMarketGaveSmthDisplay = dbResearchObj.FullATRMarketGaveSmth;
            CommentDisplay = dbResearchObj.Comment;
        }
        public List<string>? ScreenshotsUrls { get; set; }

        public SideType SideTypeDisplay { get; set; }

        /// <summary>
        ///  Risk to reward ratio 1:1.
        /// </summary>
        public string OneToOneHitOnDisplay { get; set; }

        /// <summary>
        ///  Risk to reward ratio 1:3.
        /// </summary>

        public string IsOneToThreeHitDisplay { get; set; }

        /// <summary>
        ///  Risk to reward ratio 1:5.
        /// </summary>

        public string IsOneToFiveHitDisplay { get; set; }

        public string IsBreakevenDisplay { get; set; }

        public string IsLossDisplay { get; set; }

        /// <summary>
        ///  Maximum risk to reward ratio. The number is always in ratio 1:MaxRR (e.g. 1:3)
        /// </summary>
        public int MaxRRDisplay { get; set; }

        /// <summary>
        ///  Market gave something - the trade was a loss but it moved in my favor before hitting the stop.
        /// </summary>
        public string MarketGaveSmthDisplay { get; set; }

        public string IsEntryAfter3To5BarsDisplay { get; set; }

        public string IsEntryAfter5BarsDisplay { get; set; }

        public string IsEntryAtPreviousSwingOnTriggerDisplay { get; set; }

        public string IsEntryBeforePreviousSwingOnTriggerDisplay { get; set; }

        public string IsEntryBeforePreviousSwingOn4HDisplay { get; set; }

        public string IsEntryBeforePreviousSwingOnDDisplay { get; set; }

        public string IsMomentumTradeDisplay { get; set; }

        public string IsTrendTradeDisplay { get; set; }

        public string IsTriggerTrendingDisplay { get; set; }

        public string Is4HTrendingDisplay { get; set; }

        public string IsDTrendingDisplay { get; set; }

        public string IsEntryAfteriBarDisplay { get; set; }

        public string IsSignalBarStrongReversalDisplay { get; set; }

        public string IsSignalBarInTradeDirectionDisplay { get; set; }

        public int FullATROneToOneHitOnDisplay { get; set; }

        public string IsFullATROneToThreeHitDisplay { get; set; }

        public string IsFullATROneToFiveHitDisplay { get; set; }

        public string IsFullATRBreakevenDisplay { get; set; }

        public string IsFullATRLossDisplay { get; set; }

        public int FullATRMaxRRDisplay { get; set; }

        public bool FullATRMarketGaveSmthDisplay { get; set; }

        public string? CommentDisplay { get; set; }
    }
}
