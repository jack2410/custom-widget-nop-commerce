using System;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Cms;
using Nop.Services.Logging;

namespace Nop.Plugin.Widget.DiscountAlert.Services
{
    /// <summary>
    /// Represents the plugin service implementation
    /// </summary>
    public class DiscountAlertService
    {
        #region Fields

        private readonly DiscountAlertSettings _discountAlertSettings;
        private readonly ILogger _logger;
        private readonly IStoreContext _storeContext;
        private readonly IWidgetPluginManager _widgetPluginManager;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public DiscountAlertService(DiscountAlertSettings discountAlertSettings,
            ILogger logger,
            IStoreContext storeContext,
            IWidgetPluginManager widgetPluginManager,
            IWorkContext workContext)
        {
            _discountAlertSettings = discountAlertSettings;
            _logger = logger;
            _storeContext = storeContext;
            _widgetPluginManager = widgetPluginManager;
            _workContext = workContext;
        }

        #endregion
    }
}