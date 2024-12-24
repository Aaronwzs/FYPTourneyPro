using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Entities.UserM;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Organizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
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
        private readonly IRepository<CustomUser, Guid> _customUserRepository;


        public List<CategoryDto> Categories { get; set; }
        public List<IdentityUser> Users { get; set; }
        public List<RegistrationDto> Registrations { get; set; } = new();
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public Guid TournamentId { get; set; }
        public Guid categoryId { get; set; }
        public string FullName { get; set; }

        public indexModel(RegistrationAppService registrationAppService,
            CategoryAppService categoryAppService,
            IIdentityUserRepository userRepository,
             IRepository<CustomUser, Guid> customUserRepository,
            ICurrentUser currentUser) // Inject ICurrentUser
        {
            _registrationAppService = registrationAppService;
            _categoryAppService = categoryAppService;
            _userRepository = userRepository;
            _customUserRepository = customUserRepository;
            _currentUser = currentUser; // Set ICurrentUser
        }


        public async Task OnGetAsync(Guid tournamentId)
        {
            UserId = _currentUser.Id.Value; // Get UserId from ICurrentUser
            TournamentId = tournamentId;

            // Get current user
            if (_currentUser.IsAuthenticated)
            {
               
                var currentUser = await _userRepository.GetAsync(UserId);
               var customUser = await _customUserRepository.FirstOrDefaultAsync(x => x.UserId == UserId);
                if (customUser == null)
                {
                    throw new EntityNotFoundException($"No CustomUser found with UserId: {UserId}");
                }
                UserName = currentUser.UserName;
                FullName = customUser.FullName;
            }

            Categories = await _categoryAppService.GetListByTournamentIdAsync(tournamentId);


            //tournamentId = TournamentId;
            
            //Registrations = await _registrationAppService.GetRegistrationListByCategoryAsync(CategoryId);
            
        }
    }

    
       
}

