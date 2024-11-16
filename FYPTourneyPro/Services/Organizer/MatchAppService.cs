using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class MatchAppService : FYPTourneyProAppService
    {
        private readonly IRepository<Match, Guid> _matchRepository;

        public MatchAppService(IRepository<Match, Guid> matchRepository)
        {
            _matchRepository = matchRepository;
        }
        // Fetch matches for a specific category
        public async Task<List<MatchDto>> GetMatchesByCategoryIdAsync(Guid categoryId)
        {
            // Fetch all matches for the given CategoryId
            var matches = await _matchRepository.GetListAsync(m => m.CategoryId == categoryId);

            // Convert the matches to DTOs
            var matchDtos = matches.Select(m => new MatchDto
            {
                Id = m.Id,
                round = m.round,
                courtNum = m.courtNum,
                startTime = m.startTime,
                endTime = m.endTime,
                CategoryId = m.CategoryId
            }).ToList();

            return matchDtos;
        }
    }
}
