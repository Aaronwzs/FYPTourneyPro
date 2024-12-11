using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Entities.User;
using FYPTourneyPro.Services.Dtos.Organizer;
using FYPTourneyPro.Services.Dtos.User;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;

namespace FYPTourneyPro.Services.Users
{
    public class AppFormAppService : ApplicationService
    {
        private readonly IRepository<AppForm, Guid> _applicationFormRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<IdentityRole, Guid> _roleRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityUserRepository _userRepository;

        public AppFormAppService(IRepository<AppForm, Guid> applicationFormRepository, ICurrentUser currentUser,
             IRepository<IdentityRole, Guid> roleRepository,
     IdentityUserManager userManager,
    IIdentityUserRepository userRepository)
        {
            _applicationFormRepository = applicationFormRepository;
            _currentUser = currentUser;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<AppFormDto> CreateAsync(AppFormDto input)
        {
            var application = new AppForm
            {
               
                UserId = _currentUser.Id.GetValueOrDefault(),
                RequestedRole = input.RequestedRole,
                Description = input.Description,
                IsApproved = false,
                IsRejected = false
            };

            await _applicationFormRepository.InsertAsync(application);

            return new AppFormDto
            {
                UserId =application.UserId,
                RequestedRole = application.RequestedRole,
                Description = application.Description,
                IsApproved = false,
                IsRejected = false
            };
        }

        public async Task<List<AppFormDto>> GetListAsync()
        {
            var applications = await _applicationFormRepository.GetListAsync();
            //return ObjectMapper.Map<List<AppForm>, List<AppFormDto>>(applications);

            return applications
                .Select(t => new AppFormDto
                {
                    Id = t.Id,
                   UserId = t.UserId,
                   RequestedRole=t.RequestedRole,
                   Description = t.Description,
                   IsApproved = t.IsApproved,
                   IsRejected = t.IsRejected
                }).ToList();
        }

        public async Task ApproveAsync(Guid id)
        {
            var application = await _applicationFormRepository.GetAsync(id);
            application.IsApproved = true;

            // If the role is organizer, update the user's role
            if (application.RequestedRole == "organizer" || application.RequestedRole == "referee")
            {

                // Fetch the requested role
                var user = await _userManager.GetByIdAsync(application.UserId);

                if (user == null)
                {
                    throw new BusinessException("User not found.");
                }
                // Check if the role exists
                var role = await _roleRepository.FirstOrDefaultAsync(r => r.Name == application.RequestedRole);
                if (role == null)
                {
                    throw new BusinessException($"Role '{application.RequestedRole}' does not exist.");
                }

                // Assign the role to the user
                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    throw new BusinessException("Failed to assign role to user.");
                }
            }

            await _applicationFormRepository.UpdateAsync(application);
            
        }

        public async Task RejectAsync(Guid id)
        {
            var application = await _applicationFormRepository.GetAsync(id);
            application.IsRejected = true;
            await _applicationFormRepository.UpdateAsync(application);
        }
    }

}
