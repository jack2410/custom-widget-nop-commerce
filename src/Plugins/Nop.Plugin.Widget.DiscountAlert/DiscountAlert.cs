using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using Nop.Services.Plugins;
using Nop.Web.Framework.Components;
using Nop.Services.Cms;
using Nop.Core.Domain.Cms;
using Nop.Services.Configuration;
using Nop.Web.Models.ShoppingCart;
using Nop.Web.Framework.Infrastructure;
using Nop.Services.Stores;
using Nop.Services.Localization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;

namespace Nop.Plugin.Widget.DiscountAlert
{
    public class DiscountAlertWidget : BasePlugin, IWidgetPlugin
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly ILocalizationService _localizationService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly DiscountAlertSettings _discountAlertSettings;
        #endregion

        #region Ctor
        public DiscountAlertWidget(
            ISettingService settingService, 
            IStoreService storeService, 
            ILocalizationService localizationService,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            DiscountAlertSettings discountAlertSettings
        )
        {
            _settingService = settingService;
            _storeService = storeService;
            _localizationService = localizationService;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _discountAlertSettings = discountAlertSettings;
        }
        #endregion

        #region Methods
        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(DiscountAlertDefaults.ConfigurationRouteName);
        }
        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            if (widgetZone == null)
                throw new ArgumentNullException(nameof(widgetZone));

            return DiscountAlertDefaults.VIEW_COMPONENT;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the widget zones
        /// </returns>
        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { _discountAlertSettings.WidgetZone });
        }
        public override async Task InstallAsync()
        {
            //Logic during installation goes here...
            await _settingService.SaveSettingAsync(new DiscountAlertSettings
            {
                WidgetZone = PublicWidgetZones.DiscountAlert
            });

            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widget.DiscountAlert.Fields.Enabled"] = "Enable",
                ["Plugins.Widget.DiscountAlert.Fields.Enabled.Hint"] = "Check to activate this widget.",
                ["Plugins.Widget.DiscountAlert.Fields.DiscountRange.Required"] = "Discount range is required",
                ["Plugins.Widget.DiscountAlert.Fields.DiscountPercentage.Required"] = "Discount percentage is required"
            });

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            //Logic during uninstallation goes here...
            await _settingService.DeleteSettingAsync<DiscountAlertSettings>();

            var stores = await _storeService.GetAllStoresAsync();
            var storeIds = new List<int> { 0 }.Union(stores.Select(store => store.Id));
            foreach (var storeId in storeIds)
            {
                var widgetSettings = await _settingService.LoadSettingAsync<WidgetSettings>(storeId);
                widgetSettings.ActiveWidgetSystemNames.Remove(DiscountAlertDefaults.SystemName);
                await _settingService.SaveSettingAsync(widgetSettings);
            }

            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widget.DiscountAlert");

            await base.UninstallAsync();
        }
        #endregion

        #region Properties
        public bool HideInWidgetList => false;
        #endregion
    }
}
