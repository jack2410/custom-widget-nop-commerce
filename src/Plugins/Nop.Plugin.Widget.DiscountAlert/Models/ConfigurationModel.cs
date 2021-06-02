using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widget.DiscountAlert.Models
{
    /// <summary>
    /// Represents configuration model
    /// </summary>
    public record ConfigurationModel : BaseNopModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Enable")]
        public bool Enabled { get; set; }
        public bool Enabled_OverrideForStore { get; set; }

        [NopResourceDisplayName("Discount range")]
        public double DiscountRange { get; set; }
        public bool DiscountRange_OverrideForStore { get; set; }

        [NopResourceDisplayName("Discount percentage")]
        public double DiscountPercentage { get; set; }
        public bool DiscountPercentage_OverrideForStore { get; set; }

        public string Url { get; set; }

        #endregion
    }
}