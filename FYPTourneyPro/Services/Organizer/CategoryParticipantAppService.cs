
using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer.CategoryParticipant;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class CategoryParticipantAppService : FYPTourneyProAppService
    {
        private readonly IRepository<CategoryParticipant, Guid> _categoryParticipantRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<PlayerRegistration, Guid> _playerRegistrationRepository;


        public CategoryParticipantAppService(IRepository<CategoryParticipant, Guid> categoryPaticipantRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<PlayerRegistration, Guid> playerRegistrationRepository)
        {
            _categoryParticipantRepository = categoryPaticipantRepository;
            _categoryRepository = categoryRepository;
            _playerRegistrationRepository = playerRegistrationRepository;
        }

        public async Task<List<CategoryParticipantDto>> GetListAsync()
        {
            var categoriesParticipant = await _categoryParticipantRepository.GetListAsync();
            return categoriesParticipant
                .Select(categoryParticipant => new CategoryParticipantDto
                {
                    Id = categoryParticipant.Id,
                    Seed = categoryParticipant.Seed,
                    CategoryId = categoryParticipant.CategoryId,
                    PlayerRegistrationId = categoryParticipant.PlayerRegistrationId


                }).ToList();
        }


        public async Task CreatefromPlyerRegAsync(Guid categoryId) 
            // fetches the PlayerRegistration records based on CategoryId, creates CategoryParticipant records with Seed set to 0, and saves them into the CategoryParticipant table
        {
            // Fetch all player registrations for the given category
            var registrations = await _playerRegistrationRepository.GetListAsync(r => r.CategoryId == categoryId);

            foreach (var registration in registrations)
            {
                // Check if a CategoryParticipant already exists for this PlayerRegistrationId and CategoryId
                var existingParticipant = await _categoryParticipantRepository
                    .FirstOrDefaultAsync(p => p.PlayerRegistrationId == registration.Id && p.CategoryId == categoryId);

                // If no existing participant, create a new one with Seed 0
                if (existingParticipant == null)
                {
                    var categoryParticipant = new CategoryParticipant
                    {
                        Seed = 0, // Set Seed to 0
                        CategoryId = categoryId,
                        PlayerRegistrationId = registration.Id
                    };

                    // Insert the new CategoryParticipant
                    await _categoryParticipantRepository.InsertAsync(categoryParticipant);
                }
            }
        }

        public async Task<List<CategoryParticipantDto>> GetParticipantsWithDetailsAsync(Guid categoryId)
        //fetches the CategoryParticipant records for the specified CategoryId, retrieves the corresponding PlayerName from PlayerRegistration, and returns a list of CategoryParticipantDto objects
        {
            var participants = await _categoryParticipantRepository.GetListAsync(p => p.CategoryId == categoryId);

            var result = new List<CategoryParticipantDto>();

            foreach (var participant in participants)
            {
                var playerRegistration = await _playerRegistrationRepository.GetAsync(participant.PlayerRegistrationId);

                result.Add(new CategoryParticipantDto
                {
                    Id = participant.Id,
                    Seed = participant.Seed, // Seed value (which is 0)
                    CategoryId = participant.CategoryId,
                    PlayerRegistrationId = participant.PlayerRegistrationId,
                    PlayerName = playerRegistration.UserName // Fetch PlayerName from PlayerRegistration
                });
            }

            return result;
        }



        public async Task DeleteAsync(Guid id)
        {
            await _categoryParticipantRepository.DeleteAsync(id);
        }



       

    }
}

