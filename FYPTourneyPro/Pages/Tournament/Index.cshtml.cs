using FYPTourneyPro.Services.Dtos.Organizer.Tournament;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.Tournament
{
    public class IndexModel : PageModel
    {
        public List<TournamentDto> Tournaments { get; set; } = new();
        public List<TournamentDto> UserTournaments { get; set; } = new();

        private readonly TournamentAppService _tournamentAppService;

        public IndexModel(TournamentAppService tournamentAppService)
        {
            _tournamentAppService = tournamentAppService;
        }

        public async Task OnGetAsync()
        {
            Tournaments = await _tournamentAppService.GetListAsync();
            UserTournaments = await _tournamentAppService.GetListAsyncUid();
        }
    }
}
