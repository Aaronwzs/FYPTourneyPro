using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace FYPTourneyPro.Pages.TourRegistration
{
    public class indexModel : PageModel
    {
        private readonly RegistrationAppService _registrationAppService;
        private readonly CategoryAppService _categoryAppService;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public List<CategoryDto> Categories { get; set; }
        public List<IdentityUser> Users { get; set; }
        public List<RegistrationDto> Registrations { get; set; } = new();
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid categoryId { get; set; }

        public indexModel(RegistrationAppService registrationAppService,
            CategoryAppService categoryAppService,
            IIdentityUserRepository userRepository,
            ICurrentUser currentUser) // Inject ICurrentUser
        {
            _registrationAppService = registrationAppService;
            _categoryAppService = categoryAppService;
            _userRepository = userRepository;
            _currentUser = currentUser; // Set ICurrentUser
        }


        public async Task OnGetAsync(Guid CategoryId)
        {
            categoryId = CategoryId;
           

            // Get current user
            if (_currentUser.IsAuthenticated)
            {
                UserId = _currentUser.Id.Value; // Get UserId from ICurrentUser
                var currentUser = await _userRepository.GetAsync(UserId);
                UserName = currentUser.UserName;
            }
            
            Registrations = await _registrationAppService.GetRegistrationListByCategoryAsync(CategoryId);
        }
    }
       
}

