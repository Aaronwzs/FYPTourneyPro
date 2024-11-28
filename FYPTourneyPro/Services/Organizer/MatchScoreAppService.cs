using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp;
using FYPTourneyPro.Entities.Organizer;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Services.Dtos.Organizer;

namespace FYPTourneyPro.Services.Organizer
{
    public class MatchScoreAppService : FYPTourneyProAppService
    {
        private readonly IRepository<MatchParticipant, Guid> _matchParticipantRepository;
        private readonly IRepository<MatchScore, Guid> _matchScoreRepository;
        private readonly IRepository<Participant, Guid> _participantRepository;


        public MatchScoreAppService(IRepository<MatchParticipant, Guid> matchParticipantRepository, IRepository<MatchScore, Guid> matchScoreRepository,
            IRepository<Participant,Guid> participantRepository)
        {
            _matchParticipantRepository = matchParticipantRepository;
            _matchScoreRepository = matchScoreRepository;
            _participantRepository = participantRepository;
        }

        public async Task SaveScoreAsync(MatchScoreDto input)
        {
            var matchParticipant = await _matchParticipantRepository.GetAsync(input.MatchParticipantId);
            var winnerId = Guid.Parse(input.WinnerId.ToString());

            if(winnerId == matchParticipant.UserId)
            {
                matchParticipant.IsWinner = true;
            }
            await _matchParticipantRepository.UpdateAsync(matchParticipant);

            await _matchScoreRepository.InsertAsync(new MatchScore()
            {
                MatchParticipantId = input.MatchParticipantId,
                Set1Score = int.Parse(input.Set1Score),
                Set2Score = int.Parse(input.Set2Score),
                Set3Score = input.Set3Score != null ? int.Parse(input.Set3Score) : 0
            });

           
        }

    }
}
