using FYPTourneyPro.Services.Dtos.Organizer.Category;
using FYPTourneyPro.Services.Dtos.Organizer.PlayerRegistration;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.Category
{
    public class IndexModel : PageModel
    {

        public List<PlayerRegDto> Registrations { get; set; } = new();
        public List<CategoryDto> Categories { get; set; } = new();
        public Guid TournamentId { get; set; }

        private readonly CategoryAppService _categoryAppService;
        private readonly PlayerRegistrationAppService _playerRegistrationAppService;


        public IndexModel(CategoryAppService categoryAppService, PlayerRegistrationAppService playerRegistrationAppService)
        {
            _categoryAppService = categoryAppService;
            _playerRegistrationAppService = playerRegistrationAppService;

        }

        public async Task OnGetAsync(Guid tournamentId)
        {
            TournamentId = tournamentId;
            Categories = await _categoryAppService.GetListByTournamentAsync(tournamentId);
            Registrations = await _playerRegistrationAppService.GetListByTournamentAsync(tournamentId);

        }
    }
}
