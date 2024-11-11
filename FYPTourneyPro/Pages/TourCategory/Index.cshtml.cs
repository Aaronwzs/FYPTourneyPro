using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.TourCategory
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

        // Handle Category Creation via AJAX
        public async Task<IActionResult> OnPostCreateCategoryAsync([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Invalid category data.");
            }

            var createdCategory = await _categoryAppService.CreateAsync(categoryDto);
            return new JsonResult(createdCategory);
        }

    }
}
