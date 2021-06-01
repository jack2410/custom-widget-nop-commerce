using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Nop.Services.Plugins;
using Nop.Web.Framework.Components;
using Nop.Services.Cms;

using Nop.Web.Models.ShoppingCart;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;

namespace Nop.Plugin.Widget.DiscountAlert
{
    public class DiscountAlertWidget : BasePlugin, IWidgetPlugin
    {
        public bool HideInWidgetList => false;

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "DiscountAlertWidget";
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
            return Task.FromResult<IList<string>>(new List<string> { "discount_alert" });
        }
        public override async Task InstallAsync()
        {
            //Logic during installation goes here...

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            //Logic during uninstallation goes here...

            await base.UninstallAsync();
        }
    }

    [ViewComponent(Name = "DiscountAlertWidget")]
    public class DiscountAlertWidgetViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, IList<ShoppingCartModel.ShoppingCartItemModel> additionalData)
        {
            const double PRICE_TO_DISCOUNT = 3000;
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

            string alert = string.Empty;
            if (totalPrice >= PRICE_TO_DISCOUNT)
            {
                alert = $"<h3 style=\"margin: 8px 0; color: tomato\">You get {totalPrice * 0.05}{currency} discount</h3>";
            }
            else
            {
                alert = $"<h3 style=\"margin: 8px 0; color: tomato\">You need {PRICE_TO_DISCOUNT - totalPrice}{currency} more to get 5% total bill discount</h3>";
            }
            return new HtmlContentViewComponentResult(new HtmlString(alert));
        }
    }
}
