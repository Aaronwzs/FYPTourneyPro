using FYPTourneyPro.Services.Dtos.Organizer.Category;
using FYPTourneyPro.Services.Dtos.Organizer.CategoryParticipant;
using FYPTourneyPro.Services.Dtos.Organizer.PlayerRegistration;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.Category
{
    public class CatPartList : PageModel
    {
        public List<CategoryParticipantDto> Participants { get; set; } = new();
        public CategoryDto Category { get; set; } = new();

        private readonly CategoryParticipantAppService _categoryParticipantAppService;
        private readonly CategoryAppService _categoryAppService;

        public CatPartList(
       CategoryParticipantAppService categoryParticipantAppService,
       CategoryAppService categoryAppService)
        {
            _categoryParticipantAppService = categoryParticipantAppService;
            _categoryAppService = categoryAppService;
        }

        public async Task OnGetAsync(Guid categoryId)
        {
            // Fetch the category details by ID
            Category = await _categoryAppService.GetAsync(categoryId);

            // Create CategoryParticipants from PlayerRegistrations 
            await _categoryParticipantAppService.CreatefromPlyerRegAsync(categoryId);

            // Fetch participants with PlayerName and Seed (Seed will be 0)
            Participants = await _categoryParticipantAppService.GetParticipantsWithDetailsAsync(categoryId);
        }

    }
}
