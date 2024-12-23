using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace FYPTourneyPro.Components.Notification
{
    public class NotificationViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Components/Notification/NotificationDefault.cshtml");
        }

    }
}
