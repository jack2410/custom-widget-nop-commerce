using FluentValidation;
using Nop.Plugin.Widget.DiscountAlert.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.AccessiBe.Validators
{
    /// <summary>
    /// Represents configuration model validator
    /// </summary>
    public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
    {
        #region Ctor

        public ConfigurationValidator(ILocalizationService localizationService)
        {
            RuleFor(model => model.DiscountRange)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widget.DiscountAlert.Fields.DiscountRange.Required"))
                .When(model => model.Enabled);

            RuleFor(model => model.DiscountPercentage)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widget.DiscountAlert.Fields.DiscountPercentage.Required"))
                .When(model => model.Enabled);
        }

        #endregion
    }
}