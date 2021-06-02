using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using Nop.Web.Models.ShoppingCart;
using Nop.Plugin.Widget.DiscountAlert.Services;
using Nop.Plugin.Widget.DiscountAlert.Models;

namespace Nop.Plugin.Widget.DiscountAlert.Components
{
    /// <summary>
    /// Represents the view component to place a widget into pages
    /// </summary>
    [ViewComponent(Name = DiscountAlertDefaults.VIEW_COMPONENT)]
    public class DiscountAlertViewComponent : NopViewComponent
    {
        #region Fields

        private readonly DiscountAlertService _discountAlertService;
        private readonly DiscountAlertSettings _discountAlertSettings;

        #endregion

        #region Ctor

        public DiscountAlertViewComponent(DiscountAlertService discountAlertService,
            DiscountAlertSettings discountAlertSettings)
        {
            _discountAlertService = discountAlertService;
            _discountAlertSettings = discountAlertSettings;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <param name="additionalData">Additional data</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public IViewComponentResult Invoke(string widgetZone, IList<ShoppingCartModel.ShoppingCartItemModel> additionalData)
        {
            DiscountStatus model = _discountAlertService.CalculateDiscount(additionalData);
            return View("~/Plugins/Widget.DiscountAlert/Views/DiscountAlertMessage.cshtml", model);
        }

        #endregion
    }
}