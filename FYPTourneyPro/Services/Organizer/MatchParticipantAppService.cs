using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace FYPTourneyPro.Services.Organizer
{
    public class MatchParticipantAppService : ApplicationService
    {
        private readonly IRepository<Participant, Guid> _participantRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Registration, Guid> _registrationRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IRepository<Match, Guid> _matchRepository;
        private readonly IRepository<MatchParticipant, Guid> _matchParticipantRepository;

        public MatchParticipantAppService(
            IRepository<Match, Guid> matchRepository,
            IRepository<MatchParticipant, Guid> matchParticipantRepository,
            IRepository<Participant, Guid> participantRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Registration, Guid> registrationRepository,
            IIdentityUserRepository userRepository
            )
        {
            _participantRepository = participantRepository;
            _matchRepository = matchRepository;
            _participantRepository = participantRepository;
            _categoryRepository = categoryRepository;
            _registrationRepository = registrationRepository;
            _userRepository = userRepository;
            _matchParticipantRepository = matchParticipantRepository;
        }

        public async Task<List<IGrouping<Guid, MatchParticipant>>> GenerateDrawAsync(MatchParticipantDto input)
        {
            var category = await _categoryRepository.GetAsync(input.CategoryId);
            var isSingle = category.isPair != null;
            var result = new List<MatchParticipantDto> { };

            // Step 1: Retrieve the Registration data to get the CategoryId
            var registrationList = await _registrationRepository.GetListAsync(r => r.CategoryId == input.CategoryId);
            //var catId = registration.CategoryId;
            //input.CategoryId = catId;
            //// Step 2: Retrieve the list of participants from the Participant table filtered by RegistrationId
            //var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == input.RegistrationId);

            var participantList = new List<Participant>();
            foreach (var registration in registrationList)
            {
                // each registration get the participants
                var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == registration.Id);

                if (isSingle)
                {
                    participantList.Add(participants.First());
                }
                else
                {
                    participants.ForEach(participant =>
                    {
                        participantList.Add(participant);
                    });
                }
            }

            var randomizedParticipants = participantList.OrderBy(x => Guid.NewGuid()).ToList();
            var totalMatches = 0;
            var matches = new List<Match>();

            // create matches
            if (isSingle)
            {
                var isEven = randomizedParticipants.Count % 2 == 0;
                var totalParticipants = isEven ? randomizedParticipants.Count : randomizedParticipants.Count + 1;
                totalMatches = totalParticipants / 2;
            }
            else
            {
                totalMatches = randomizedParticipants.Count / 4;
            }

            for (int j = 0; j < totalMatches; j++)
            {
                var match = new Match
                {
                    round = 1,
                    startTime = DateTime.Now,
                    endTime = DateTime.Now.AddMinutes(30),
                    courtNum = 1,
                    CategoryId = input.CategoryId
                };

                // Add match to the match list and insert it into the Match table
                matches.Add(match);
            }

            await _matchRepository.InsertManyAsync(matches);

            // create match participants
            var matchParticipants = new List<MatchParticipant>();

            foreach (var match in matches)
            {
                if (isSingle)
                {
                    for (int i = 0; i < randomizedParticipants.Count; i += 2)
                    {
                        var participant1 = randomizedParticipants[i];
                        var participant2 = randomizedParticipants[i + 1];

                        matchParticipants.Add(new MatchParticipant
                        {
                            MatchId = match.Id,
                            UserId = participant1.UserId
                        });

                        matchParticipants.Add(new MatchParticipant
                        {
                            MatchId = match.Id,
                            UserId = participant2.UserId
                        });
                    }
                }
                else
                {
                    var groupedPairs = randomizedParticipants
                        .GroupBy(p => p.PairId)
                        .Where(g => g.Count() == 2)  // Ensure each pairId has 2 participants
                        .ToList();

                    var randomizedPairs = groupedPairs.OrderBy(x => Guid.NewGuid()).ToList();

                    for (int i = 0; i < randomizedPairs.Count; i += 2)
                    {
                        var pair1 = randomizedPairs[i];
                        var pair2 = randomizedPairs[i+1];

                        matchParticipants.Add(new MatchParticipant
                        {
                            MatchId = match.Id,
                            UserId = pair1.First().UserId,
                            PairId = pair1.First().PairId
                        });

                        matchParticipants.Add(new MatchParticipant
                        {
                            MatchId = match.Id,
                            UserId = pair1.Last().UserId,
                            PairId = pair1.Last().PairId
                        });

                        matchParticipants.Add(new MatchParticipant
                        {
                            MatchId = match.Id,
                            UserId = pair2.First().UserId,
                            PairId = pair2.First().PairId
                        });

                        matchParticipants.Add(new MatchParticipant
                        {
                            MatchId = match.Id,
                            UserId = pair2.Last().UserId,
                            PairId = pair2.Last().PairId
                        });
                    }

                }

            }

            await _matchParticipantRepository.InsertManyAsync(matchParticipants);


            return matchParticipants.GroupBy(mp => mp.MatchId).ToList();
        }

        // Get participants for a specific match
        public async Task<List<MatchParticipantDto>> GetMatchParticipantsByMatchIdAsync(Guid matchId)
        {
            // Retrieve all participants for the specified matchId
            var matchParticipants = await _matchParticipantRepository.GetListAsync(mp => mp.MatchId == matchId);

            // Convert the entities into MatchParticipantDto
            var matchParticipantDtos = matchParticipants.Select(mp => new MatchParticipantDto
            {
                Id = mp.Id,
                userId = mp.UserId,
                pairId = mp.PairId,
                matchId = mp.MatchId,
                isWinner = mp.IsWinner
            }).ToList();

            return matchParticipantDtos;
        }



    }
}

