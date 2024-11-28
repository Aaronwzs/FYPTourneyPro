using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;

using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.TourCategory
{
    public class CatPartListModel : PageModel
    {
        private readonly CategoryAppService _categoryAppService;
        private readonly RegistrationAppService _registrationAppService;
       private readonly MatchParticipantAppService _matchParticipantAppService;
        private readonly MatchAppService _matchAppService;
        private readonly MatchScoreAppService _matchScoreAppService;

        [BindProperty(SupportsGet = true)]
        public Guid categoryId { get; set; }
        public List<RegistrationDto> Registrations { get; set; } 
        public CategoryDto Category { get; set; }
        public List<ParticipantDto> Participants { get; set; }
        public List<MatchDto> Matches { get; set; }
        public List<MatchParticipantDto> MatchParticipants { get; set; }
        public string Message { get; set; }


        public CatPartListModel(CategoryAppService categoryAppService, RegistrationAppService registrationAppService 
            ,MatchParticipantAppService matchParticipantAppService,
            MatchAppService matchAppService,
            MatchScoreAppService matchScoreAppService
            )
        {
            _categoryAppService = categoryAppService;
            _registrationAppService = registrationAppService;
            _matchParticipantAppService = matchParticipantAppService;
            _matchAppService = matchAppService;
            _matchScoreAppService= matchScoreAppService;

        }

        public async Task OnGetAsync(Guid CategoryId)
        {
            categoryId = CategoryId;

            // Get category details
            Category = await _categoryAppService.GetAsync(CategoryId);


            // Get list of registrations for the category
            Registrations = await _registrationAppService.GetRegistrationListByCategoryAsync(CategoryId);



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


        // Handle the form submission when the "Generate Draw" button is clicked
        public async Task<IActionResult> OnPostGenerateDrawAsync()
        {
            try
            {
                //Call the service method to generate the draw for the category

               var matchParticipants = await _matchParticipantAppService.GenerateDrawAsync(new MatchParticipantDto
               {
                   CategoryId = categoryId
                   // Assuming we need to pass the CategoryId for the draw
               }) ;

                // Feedback message on successful draw generation
                TempData["Message"] = "Draw generated successfully!";

                // Redirect to the DrawList page to display the generated matches
                return RedirectToPage("/TourCategory/DrawList", new { categoryId = categoryId });
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the draw generation process
                TempData["Message"] = $"Error generating draw: {ex.Message}";
                // Redirect to the current page to reflect changes and display the message
                return RedirectToPage();
            }

            
        }
    }
}