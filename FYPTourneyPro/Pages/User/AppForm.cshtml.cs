using FYPTourneyPro.Services.Dtos.User;
using FYPTourneyPro.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.User
{
    public class AppFormModel : PageModel
    {
        public List<AppFormDto> ApplicationForms { get; set; } = new();

        private readonly AppFormAppService _appFormAppService;

        public AppFormModel(AppFormAppService appFormAppService)
        {
            _appFormAppService = appFormAppService;
        }

        public async Task OnGetAsync()
        {
            ApplicationForms = await _appFormAppService.GetListAsync();
        }

        public async Task<IActionResult> OnPostSubmitApplicationAsync(AppFormDto applicationForm)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _appFormAppService.CreateAsync(applicationForm);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid id)
        {
            await _appFormAppService.ApproveAsync(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(Guid id)
        {
            await _appFormAppService.RejectAsync(id);
            return RedirectToPage();
        }

    }
}
