using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Widget.DiscountAlert.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widget.DiscountAlert.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class DiscountAlertController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public DiscountAlertController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IStoreService storeService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _storeService = storeService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<DiscountAlertSettings>(storeId);
            var widgetSettings = await _settingService.LoadSettingAsync<WidgetSettings>(storeId);

            var model = new ConfigurationModel
            {
                DiscountRange = settings.DiscountRange,
                DiscountPercentage = settings.DiscountPercentage,
                Enabled = widgetSettings.ActiveWidgetSystemNames.Contains(DiscountAlertDefaults.SystemName),
                ActiveStoreScopeConfiguration = storeId
            };

            if (storeId > 0)
            {
                model.DiscountRange_OverrideForStore = await _settingService.SettingExistsAsync(settings, setting => setting.DiscountRange, storeId);
                model.DiscountPercentage_OverrideForStore = await _settingService.SettingExistsAsync(settings, setting => setting.DiscountPercentage, storeId);
                model.Enabled_OverrideForStore = await _settingService.SettingExistsAsync(widgetSettings, setting => setting.ActiveWidgetSystemNames, storeId);
            }

            //prepare store URL
            model.Url = storeId > 0
                ? (await _storeService.GetStoreByIdAsync(storeId))?.Url
                : _webHelper.GetStoreLocation();

            return View("~/Plugins/Widget.DiscountAlert/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var settings = await _settingService.LoadSettingAsync<DiscountAlertSettings>(storeId);
            var widgetSettings = await _settingService.LoadSettingAsync<WidgetSettings>(storeId);

            settings.DiscountRange = model.DiscountRange;
            settings.DiscountPercentage = model.DiscountPercentage;
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, setting => setting.DiscountRange, model.DiscountRange_OverrideForStore, storeId, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(settings, setting => setting.DiscountPercentage, model.DiscountPercentage_OverrideForStore, storeId, false);

            if (model.Enabled && !widgetSettings.ActiveWidgetSystemNames.Contains(DiscountAlertDefaults.SystemName))
                widgetSettings.ActiveWidgetSystemNames.Add(DiscountAlertDefaults.SystemName);
            if (!model.Enabled && widgetSettings.ActiveWidgetSystemNames.Contains(DiscountAlertDefaults.SystemName))
                widgetSettings.ActiveWidgetSystemNames.Remove(DiscountAlertDefaults.SystemName);
            await _settingService.SaveSettingOverridablePerStoreAsync(widgetSettings, setting => setting.ActiveWidgetSystemNames, model.Enabled_OverrideForStore, storeId, false);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion
    }
}