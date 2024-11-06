using FYPTourneyPro.Services.Dtos.Organizer.Category;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.Category
{
    public class IndexModel : PageModel
    {
        public List<CategoryDto> Categories { get; set; } = new();
        public Guid TournamentId { get; set; }

        private readonly CategoryAppService _categoryAppService;
       


        public IndexModel(CategoryAppService categoryAppService )
        {
            _categoryAppService = categoryAppService;
           
        }

        public async Task OnGetAsync(Guid tournamentId)
        {
            TournamentId = tournamentId;
            Categories = await _categoryAppService.GetListByTournamentAsync(tournamentId);

            
        }
    }
}
