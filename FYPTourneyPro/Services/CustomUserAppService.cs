using FYPTourneyPro.Entities.UserM;
using FYPTourneyPro.Services.Dtos.User;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace FYPTourneyPro.Services
{
    public class CustomUserAppService : FYPTourneyProAppService
    {
        private readonly IRepository<CustomUser, Guid> _customUserRepository;
        private readonly ICurrentUser _currentUser;

        public CustomUserAppService(IRepository<CustomUser, Guid> customUserRepository, ICurrentUser currentUser)
        {
            _customUserRepository = customUserRepository;
            _currentUser = currentUser;
        }

        public async Task<UserDto> CreateAsync(UserDto input)
        {
            if (_currentUser.IsAuthenticated)
            {
                // Validation could be added here if needed

                // Create a new user record in the custom User table
                var customUser = new CustomUser
                {
                    UserId = _currentUser.Id.Value,  // Set UserId from the logged-in user
                    FullName = input.FullName,
                    Age = input.Age,
                    Nationality = input.Nationality,
                    PhoneNumber = input.PhoneNumber
                };

                // Insert into Custom User Table
                await _customUserRepository.InsertAsync(customUser);

                return new UserDto
                {
                    Id = customUser.UserId,
                    FullName = customUser.FullName,
                    Age = customUser.Age,
                    Nationality = customUser.Nationality,
                    PhoneNumber = customUser.PhoneNumber,
                    UserName = input.UserName,
                    EmailAddress = input.EmailAddress,
                    Password = input.Password
                };
            }
            else
            {
                throw new Exception("User must be logged in to register a custom user.");
            }
        }

        public async Task<UserDto> GetAsync(Guid userid)
        {
            //var customUser = await _customUserRepository.GetAsync(userid);

            var customUser = await _customUserRepository.FirstOrDefaultAsync(x =>x.UserId == userid);
            if (customUser == null)
            {
                throw new EntityNotFoundException($"No CustomUser found with UserId: {userid}");
            }
            return new UserDto
            {
                Id = customUser.Id,
                UserId = customUser.UserId,
                FullName = customUser.FullName,
                Age = customUser.Age,
                Nationality = customUser.Nationality,
                PhoneNumber = customUser.PhoneNumber
            };
        }

        public async Task<List<UserDto>> GetListAsync(Guid userId)
        {
            // Fetch the custom users based on the provided IDs
            var customUsers = await _customUserRepository.GetListAsync(cu =>cu.UserId == userId );

            // Map the custom users to UserDto
            var userDtos = customUsers.Select(customUser => new UserDto
            {
                Id=customUser.Id,
                UserId = customUser.UserId,
                FullName = customUser.FullName,
                Age = customUser.Age,
                Nationality = customUser.Nationality,
                PhoneNumber = customUser.PhoneNumber
            }).ToList();

            return userDtos;
        }
    }
}
