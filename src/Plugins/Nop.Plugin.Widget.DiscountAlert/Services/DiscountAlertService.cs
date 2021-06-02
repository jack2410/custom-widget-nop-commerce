using System;
using System.Collections.Generic;
using Nop.Web.Models.ShoppingCart;
using Nop.Plugin.Widget.DiscountAlert.Models;

namespace Nop.Plugin.Widget.DiscountAlert.Services
{
    /// <summary>
    /// Represents the plugin service implementation
    /// </summary>
    public class DiscountAlertService
    {
        #region Fields
        private readonly DiscountAlertSettings _discountAlertSettings;
        #endregion

        #region Ctor
        public DiscountAlertService(DiscountAlertSettings discountAlertSettings)
        {
            _discountAlertSettings = discountAlertSettings;
        }
        #endregion

        #region Methods
        public DiscountStatus CalculateDiscount(IList<ShoppingCartModel.ShoppingCartItemModel> additionalData)
        {
            double DISCOUNT_RANGE = _discountAlertSettings.DiscountRange;
            double DISCOUNT_PERCENTAGE = _discountAlertSettings.DiscountPercentage / 100;

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
            return new DiscountStatus
            {
                Price = haveDiscount
                    ? totalPrice * DISCOUNT_PERCENTAGE
                    : DISCOUNT_RANGE - totalPrice,
                DiscountPercentage = DISCOUNT_PERCENTAGE,
                Currency = currency,
                HaveDiscount = haveDiscount
            };
        }
        #endregion
    }
}