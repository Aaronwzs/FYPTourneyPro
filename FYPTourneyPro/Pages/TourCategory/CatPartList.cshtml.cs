using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Dtos.User;
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
        private readonly CustomUserAppService _customUserAppService;


        [BindProperty(SupportsGet = true)]
        public Guid categoryId { get; set; }
        public List<RegistrationDto> Registrations { get; set; } 
        public CategoryDto Category { get; set; }
        public List<ParticipantDto> Participants { get; set; }
        public List<MatchDto> Matches { get; set; }
        public List<MatchParticipantDto> MatchParticipants { get; set; }
        public List<MatchScoreDto> MatchScores { get; set; }
        public string Message { get; set; }
        public Guid userId1 { get; set; }
        public Guid? userId2 { get; set; }

        public List<UserDto> customUser { get; set; }


        public CatPartListModel(CategoryAppService categoryAppService, 
            RegistrationAppService registrationAppService,
            MatchParticipantAppService matchParticipantAppService,
            MatchAppService matchAppService,
            MatchScoreAppService matchScoreAppService,
            CustomUserAppService customUserAppService
            )
        {
            _categoryAppService = categoryAppService;
            _registrationAppService = registrationAppService;
            _matchParticipantAppService = matchParticipantAppService;
            _matchAppService = matchAppService;
            _matchScoreAppService= matchScoreAppService;
            _customUserAppService = customUserAppService;

        }

        public async Task OnGetAsync(Guid CategoryId)
        {
            categoryId = CategoryId;

            // Get category details
            Category = await _categoryAppService.GetAsync(CategoryId);

            
            



            // Get list of registrations for the category
            Registrations = await _registrationAppService.GetRegistrationListByCategoryAsync(CategoryId);

            //for (int i = 0; i < Registrations.Count; i++)
            //{
            //    userId1 = Registrations[i].UserId1;
            //    userId2 = Registrations[i].UserId2;

            //    customUser = await _customUserAppService.GetListAsync(userId1);
            //    List<UserDto> customUser2 = null;
            //    if (userId2.HasValue)
            //    {
            //        customUser2 = await _customUserAppService.GetListAsync(userId2.Value);
            //    }



            //}



            // Get the generated matches for the category
            Matches = await _matchAppService.GetMatchesByCategoryIdAsync(CategoryId);

            // Get the participants for each match
            MatchParticipants = new List<MatchParticipantDto>();
            foreach (var match in Matches)
            {
                //get the matchParticipants for this match in matches( should have 2 )
                var participants = await _matchParticipantAppService.GetMatchParticipantsByMatchIdAsync(match.Id);
                MatchParticipants.AddRange(participants);

                
                //Get the userId from matchParticipants parse it to customUserAppService to get FUllName
                //var matchParticipantUserId? = MatchParticipants[0].userId;

                //var custUser = await _customUserAppService.GetAsync(matchParticipantUserId);



            }

            // Get the match scores for the matches
            MatchScores = new List<MatchScoreDto>();
            foreach (var match in Matches)
            {
                var scores = await _matchScoreAppService.GetMatchScoreAsync(match.Id);
                MatchScores.AddRange(scores);
            }

        }
        //public async Task<IActionResult> OnGetMatchParticipants()
        //{
        //    MatchParticipants = new List<MatchParticipantDto>();
        //    foreach (var match in Matches)
        //    {
        //        var participants = await _matchParticipantAppService.GetMatchParticipantsByMatchIdAsync(match.Id);
        //        MatchParticipants.AddRange(participants);
        //    }

        //    // Return as JSON
        //    return new JsonResult(MatchParticipants);
        //}


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