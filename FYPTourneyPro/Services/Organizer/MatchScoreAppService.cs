using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp;
using FYPTourneyPro.Entities.Organizer;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Services.Dtos.Organizer;
using System.Linq;
using Serilog.Filters;
using FYPTourneyPro.Migrations;
using FYPTourneyPro.Entities.UserM;
using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Services.Organizer
{
    public class MatchScoreAppService : FYPTourneyProAppService
    {
        private readonly IRepository<MatchParticipant, Guid> _matchParticipantRepository;
        private readonly IRepository<MatchScore, Guid> _matchScoreRepository;
        private readonly IRepository<Participant, Guid> _participantRepository;
        private readonly IRepository<Match, Guid> _matchRepository;
        private readonly IRepository<CustomUser, Guid> _customUserRepository;


        public MatchScoreAppService(IRepository<MatchParticipant, Guid> matchParticipantRepository, IRepository<MatchScore, Guid> matchScoreRepository,
            IRepository<Participant,Guid> participantRepository, IRepository<Match, Guid> matchRepository, IRepository<CustomUser, Guid> customUserRepostory)
        {
            _matchParticipantRepository = matchParticipantRepository;
            _matchScoreRepository = matchScoreRepository;
            _participantRepository = participantRepository;
            _matchRepository = matchRepository;
            _customUserRepository = customUserRepostory;
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

            //UserID get isWinner, if true, look for match field for this user, can get the nextmatchid fom the match, assign this user to this nextmatchId
            //assign the matchparticipant isWinner = true to the nextmatchId




               await _matchScoreRepository.InsertAsync(new MatchScore()
            {
                MatchParticipantId = input.MatchParticipantId,
                Set1Score = int.Parse(input.Set1Score),
                Set2Score = int.Parse(input.Set2Score),
                Set3Score = input.Set3Score != null ? int.Parse(input.Set3Score) : 0
            });                                
            var matchParticipantsList = new List<MatchParticipant>();

            if ( matchParticipant.IsWinner == true)
            {
                // assign this match participant into the nextMatchId
                //Get NextMatchId

                var matchId = matchParticipant.MatchId;

                var match = await _matchRepository.GetAsync(matchId);
                var nextMatchId = match.nextMatchId;

                if (nextMatchId != null)
                {

                    var matchPart = new MatchParticipant
                    {
                        MatchId = (Guid)nextMatchId,
                        UserId = matchParticipant.UserId

                    };
                    //assign this user to this next match id
                    matchParticipantsList.Add(matchPart);

                    await _matchParticipantRepository.InsertAsync(matchPart);

                }


                
            }

        }


        public async Task<List<MatchScoreDto>> GetMatchScoreAsync( Guid matchId)
        {

            var scores = new List<MatchScoreDto>();
            var custUserFN = "";

            // Fetch all participants for the given matchId
            var matchParticipants = await _matchParticipantRepository.GetListAsync(mp => mp.MatchId == matchId);

            // Check if there are any participants in the match
            if (matchParticipants.Count > 0)
            {
                // Loop through the match participants and fetch the MatchScore for each
                foreach (var mParticipant in matchParticipants)
                {
                    // Fetch the match score for the current match participant
                    var matchScore = await _matchScoreRepository.FirstOrDefaultAsync(ms => ms.MatchParticipantId == mParticipant.Id);

                    var customUser = await _customUserRepository.FirstOrDefaultAsync(cu  => cu.UserId == mParticipant.UserId);

                    if (customUser == null)
                    {
                        throw new EntityNotFoundException($"No CustomUser found with UserId: {mParticipant.UserId}");
                    }


                    custUserFN = customUser.FullName;

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
                            WinnerId = mParticipant.UserId, // Assuming WinnerId is the UserId of the MatchParticipant
                            WinnerFullName = custUserFN
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
