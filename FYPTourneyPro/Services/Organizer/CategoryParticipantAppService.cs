
using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer.CategoryParticipant;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class CategoryParticipantAppService : FYPTourneyProAppService
    {
        private readonly IRepository<CategoryParticipant, Guid> _categoryParticipantRepository;


        public CategoryParticipantAppService(IRepository<CategoryParticipant, Guid> categoryPaticipantRepository)
        {
            _categoryParticipantRepository = categoryPaticipantRepository;
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


        public async Task<CategoryParticipantDto> CreateAsync(CategoryParticipantDto input)
        {
            var participant = await _categoryParticipantRepository.InsertAsync(new CategoryParticipant
            {
                Seed = input.Seed,
                CategoryId = input.CategoryId,
                PlayerRegistrationId = input.PlayerRegistrationId
            });

            return ObjectMapper.Map<CategoryParticipant, CategoryParticipantDto>(participant);
        }



        public async Task DeleteAsync(Guid id)
        {
            await _categoryParticipantRepository.DeleteAsync(id);
        }
    }
}

