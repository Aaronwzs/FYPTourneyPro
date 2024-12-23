using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace FYPTourneyPro.Components.Chatbot
{
    public class ChatbotViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Components/Chatbot/Chatbot.cshtml");
        }
    }
}
