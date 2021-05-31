using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Nop.Services.Plugins;
using Nop.Web.Framework.Components;
using Nop.Services.Cms;

namespace Nop.Plugin.Widget.HelloWorld
{
    public class HelloWorldWidget : BasePlugin, IWidgetPlugin
    {
        public bool HideInWidgetList => false;

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "HelloWorldWidget";
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
            return Task.FromResult<IList<string>>(new List<string> { "home_page_before_categories" });
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

    [ViewComponent(Name = "HelloWorldWidget")]
    public class HelloWorldWidgetViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone)
        {
            return Content("Hello World");
        }
    }
}
