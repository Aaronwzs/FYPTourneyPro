using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp;
using FYPTourneyPro.Entities.Organizer;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Services.Dtos.Organizer;
using System.Linq;
using Serilog.Filters;

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


        public async Task<List<MatchScoreDto>> GetMatchScoreAsync( Guid matchId)
        {

            var scores = new List<MatchScoreDto>();

            // Fetch all participants for the given matchId
            var matchParticipants = await _matchParticipantRepository.GetListAsync(mp => mp.MatchId == matchId);

            // Check if there are any participants in the match
            if (matchParticipants.Count > 0)
            {
                // Loop through the match participants and fetch the MatchScore for each
                foreach (var mParticipant in matchParticipants)
                {
                    // Fetch the match score for the current match participant
                    var matchScore = await _matchScoreRepository
                        .FirstOrDefaultAsync(ms => ms.MatchParticipantId == mParticipant.Id);

                    // If a match score exists for this participant, map it to the DTO
                    if (matchScore != null)
                    {
                        var matchScoreDto = new MatchScoreDto
                        {
                            MatchParticipantId = matchScore.MatchParticipantId,
                            MatchId = mParticipant.MatchId,
                            Set1Score = matchScore.Set1Score.ToString(),
                            Set2Score = matchScore.Set2Score.ToString(),
                            Set3Score = matchScore.Set3Score.ToString(),
                            WinnerId = mParticipant.UserId // Assuming WinnerId is the UserId of the MatchParticipant
                        };

                        // Add the match score to the list
                        scores.Add(matchScoreDto);
                    }
                }
            }

            return scores;  // Return the list of match scores
        }
    }
}
