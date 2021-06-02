using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Nop.Plugin.Widget.DiscountAlert.Services;
using Nop.Plugin.Widget.DiscountAlert.Models;
using Nop.Web.Framework.Components;
using Nop.Web.Models.ShoppingCart;

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
            double DISCOUNT_RANGE = _discountAlertSettings.DiscountRange;
            double DISCOUNT_PERCENTAGE = _discountAlertSettings.DiscountPercentage / 100;

            // For example, if total price is over PRICE_TO_DISCOUNT, the discount is 5%
            double totalPrice = 0;
            string currency = string.Empty;
            foreach (var product in additionalData)
            {
                string unitPrice = product.UnitPrice;
                if (string.IsNullOrEmpty(currency))
                {
                    currency = unitPrice.Split(" ")[1];
                }
                string unitPriceRemovedCurrency = unitPrice.Split(" ")[0];
                double price = Convert.ToDouble(unitPriceRemovedCurrency.Replace(".", "").Replace(",", "."));
                totalPrice += price * product.Quantity;
            }

            bool haveDiscount = totalPrice >= DISCOUNT_PERCENTAGE;
            DiscountStatus model = new DiscountStatus
            { 
                Price = haveDiscount 
                    ? totalPrice * DISCOUNT_PERCENTAGE
                    : DISCOUNT_RANGE - totalPrice,
                DiscountPercentage = DISCOUNT_PERCENTAGE,
                Currency = currency,
                HaveDiscount = haveDiscount
            };
            return View("~/Plugins/Widget.DiscountAlert/Views/DiscountAlertMessage.cshtml", model);
        }

        #endregion
    }
}