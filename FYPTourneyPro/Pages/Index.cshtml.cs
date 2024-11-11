using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace FYPTourneyPro.Pages;

public class IndexModel : AbpPageModel
{
    private readonly TournamentAppService _tournamentAppService;
    public IndexModel(TournamentAppService tournamentAppService)
    {
        _tournamentAppService = tournamentAppService;
    }
    public List<TournamentDto> Tournaments { get; set; }

    public async Task OnGetAsync()
    {
        Tournaments = await _tournamentAppService.GetListAsync();
        Tournaments = await _tournamentAppService.GetListAsyncUid();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _tournamentAppService.DeleteAsync(id);
        return RedirectToPage();
    }
}