using Nop.Core;

namespace Nop.Plugin.Widget.DiscountAlert
{
    /// <summary>
    /// Represents plugin default vaues and constants
    /// </summary>
    public class DiscountAlertDefaults
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Widget.DiscountAlert";

        /// <summary>
        /// Gets the user agent used to request third-party services
        /// </summary>
        public static string UserAgent => $"nopcommerce-{NopVersion.CURRENT_VERSION}";

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Widget.DiscountAlert.Configure";

        /// <summary>
        /// Gets the name of the view component to place a widget into pages
        /// </summary>
        public const string VIEW_COMPONENT = "DiscountAlert";
    }
}