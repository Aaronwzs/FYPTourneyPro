using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp;

namespace FYPTourneyPro.Pages.TourCategory
{
    public class DrawListModel : PageModel
    {
        private readonly MatchParticipantAppService _matchParticipantAppService;
        private readonly MatchAppService _matchAppService;
        private readonly CategoryAppService _categoryAppService;
        private readonly MatchScoreAppService _matchScoreAppService;

        [BindProperty(SupportsGet = true)]
        public Guid CategoryId { get; set; }

        public CategoryDto Category { get; set; }
        public List<MatchDto> Matches { get; set; }
        public List<MatchParticipantDto> MatchParticipants { get; set; }

        public string Message { get; set; }

        public DrawListModel(
            MatchParticipantAppService matchParticipantAppService,
            MatchAppService matchAppService, CategoryAppService categoryAppService, RegistrationAppService registrationAppService, MatchScoreAppService matchScoreAppService)
        {
            _matchParticipantAppService = matchParticipantAppService;
            _matchAppService = matchAppService;
            _categoryAppService = categoryAppService;
            _matchScoreAppService = matchScoreAppService;
        }



        public async Task OnGetAsync(Guid categoryId)
        {
            CategoryId = categoryId;
            Category = await _categoryAppService.GetAsync(categoryId);

            // Get the generated matches for the category
            Matches = await _matchAppService.GetMatchesByCategoryIdAsync(CategoryId);

            // Get the participants for each match
            MatchParticipants = new List<MatchParticipantDto>();
            foreach (var match in Matches)
            {
                var participants = await _matchParticipantAppService.GetMatchParticipantsByMatchIdAsync(match.Id);
                MatchParticipants.AddRange(participants);
            }
        }

        public async Task<IActionResult> OnGetMatchParticipants()
        {
            MatchParticipants = new List<MatchParticipantDto>();
            foreach (var match in Matches)
            {
                var participants = await _matchParticipantAppService.GetMatchParticipantsByMatchIdAsync(match.Id);
                MatchParticipants.AddRange(participants);
            }

            // Return as JSON
            return new JsonResult(MatchParticipants);
        }

        //[HttpPost]
        //public async Task<IActionResult> OnPostSaveScoreAsync([FromBody] MatchScoreDto input)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid data.");
        //    }

        //    try
        //    {
        //        await _matchScoreAppService.SaveScoreAsync(input);
        //        return new JsonResult(new { success = true, message = "Score saved successfully!" });
        //    }
        //    catch (BusinessException ex)
        //    {
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
        //    }
        //}
    }
}