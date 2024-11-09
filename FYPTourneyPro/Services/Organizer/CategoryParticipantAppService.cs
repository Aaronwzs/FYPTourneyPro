
using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer.CategoryParticipant;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class CategoryParticipantAppService : FYPTourneyProAppService
    {
        private readonly IRepository<CategoryParticipant, Guid> _categoryParticipantRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;



        public CategoryParticipantAppService(IRepository<CategoryParticipant, Guid> categoryPaticipantRepository,
            IRepository<Category, Guid> categoryRepository
            )
        {
            _categoryParticipantRepository = categoryPaticipantRepository;
            _categoryRepository = categoryRepository;





        }
    }
}

