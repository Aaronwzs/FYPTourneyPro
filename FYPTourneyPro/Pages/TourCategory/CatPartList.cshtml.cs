using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;

using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.TourCategory
{
    public class CatPartList : PageModel
    {

        private readonly ParticipantAppService _participantAppService;
        private readonly CategoryAppService _categoryAppService;
        private readonly RegistrationAppService _registrationAppService;

        [BindProperty(SupportsGet = true)]
        public Guid categoryId { get; set; }

        public List<RegistrationDto> Registrations { get; set; } = new();
        public CategoryDto Category { get; set; }
        public List<ParticipantDto> Participants { get; set; } = new();

        public CatPartList(ParticipantAppService participantAppService, CategoryAppService categoryAppService, RegistrationAppService registrationAppService)
        {
            _participantAppService = participantAppService;
            _categoryAppService = categoryAppService;
            _registrationAppService = registrationAppService;
        }

        public async Task OnGetAsync(Guid CategoryId)
        {
            categoryId = CategoryId;

            Category = await _categoryAppService.GetAsync(CategoryId);

            Registrations = await _registrationAppService.GetRegistrationListByCategoryAsync(CategoryId);
        }

    }

}