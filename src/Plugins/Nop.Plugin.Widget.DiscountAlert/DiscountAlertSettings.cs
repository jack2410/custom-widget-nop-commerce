using Nop.Core.Configuration;

namespace Nop.Plugin.Widget.DiscountAlert
{
    /// <summary>
    /// Represents plugin settings
    /// </summary>
    public class DiscountAlertSettings : ISettings
    {
        /// <summary>
        /// Gets or sets an installation script
        /// </summary>
        public double DiscountRange { get; set; }
        public double DiscountPercentage { get; set; }

        /// <summary>
        /// Gets or sets a widget zone name to place a widget
        /// </summary>
        public string WidgetZone { get; set; }
    }
}