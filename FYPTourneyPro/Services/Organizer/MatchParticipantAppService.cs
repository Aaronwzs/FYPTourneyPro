using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Entities.UserM;
using FYPTourneyPro.Permissions;
using FYPTourneyPro.Services.Dtos.Organizer;
using Microsoft.AspNetCore.Authorization;
using System;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

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
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<CustomUser, Guid> _customUserRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IRepository<Tournament, Guid> _tournamentRepository;

        public MatchParticipantAppService(
            IRepository<Match, Guid> matchRepository,
            IRepository<MatchParticipant, Guid> matchParticipantRepository,
            IRepository<Participant, Guid> participantRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Registration, Guid> registrationRepository,
            IIdentityUserRepository userRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<CustomUser, Guid> customerUserRepository,
            IAuthorizationService authorizationService,
            IRepository<Tournament, Guid> tournamentRepository
            )
        {
            _participantRepository = participantRepository;
            _matchRepository = matchRepository;
            _participantRepository = participantRepository;
            _categoryRepository = categoryRepository;
            _registrationRepository = registrationRepository;
            _userRepository = userRepository;
            _matchParticipantRepository = matchParticipantRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _customUserRepository = customerUserRepository;
            _authorizationService = authorizationService;
            _tournamentRepository = tournamentRepository;
        }

        public async Task<List<IGrouping<Guid, MatchParticipant>>>? GenerateDrawAsync(MatchParticipantDto input)
        {
            // Check if the user has permission to delete a todo item
            var isAuthorized = await _authorizationService.IsGrantedAsync(FYPTourneyProPermissions.Draws.Generate);
            if (!isAuthorized)
            {
                throw new AbpAuthorizationException($"You are not authorized to update tournaments. Required permission: {FYPTourneyProPermissions.Draws.Generate}");
            }

            var category = await _categoryRepository.GetAsync(input.CategoryId);
            var matchlist = await _matchRepository.GetListAsync(m => m.CategoryId == input.CategoryId);
            if (matchlist.Count == 0)//there is no matches)
            {
                // perform the generate draw
           
            try
            {


                
                var isSingle = category.isPair != null;
                var result = new List<MatchParticipantDto> { };

                // Step 1: Retrieve the Registration data to get the CategoryId
                var registrationList = await _registrationRepository.GetListAsync(r => r.CategoryId == input.CategoryId);


                //var catId = registration.CategoryId;
                //input.CategoryId = catId;
                //// Step 2: Retrieve the list of participants from the Participant table filtered by RegistrationId
                //var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == input.RegistrationId);

                //new list 
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

                //scenario - 4 participants
                var randomizedParticipants = participantList.OrderBy(x => Guid.NewGuid()).ToList();
                var firstRoundMatch = 0;

                var matches = new List<Match>();
                var round = 0;

                    Random random = new Random();
                    int randomCourt = random.Next(1, 11);

                    var Match = await _matchRepository.GetAsync(input.CategoryId);

                    var tourId = category.TournamentId;

                    var tournament = await _tournamentRepository.GetAsync(tourId);

                    var startDate = tournament.StartDate;
                    var endDate = tournament.EndDate;

                    // create matches
                    if (isSingle)
                {

                    //make sure it is even first 
                    var isEven = randomizedParticipants.Count % 2 == 0;
                    var totalParticipants = isEven ? randomizedParticipants.Count : randomizedParticipants.Count + 1;
                    // firstRoundMatch = totalParticipants / 2;



                    do
                    {
                        //4 participants = 2 matches( semi ) r1 and 1 match( finals ) r2

                        totalParticipants = totalParticipants / 2;
                        round++;
                        //add new match

                        // var newMatches = new List<Match>();

                        for (int j = 0; j < totalParticipants; j++)
                        {
                            var match = new Match
                            {
                                round = round,
                                startTime = startDate.AddMinutes(30),
                                endTime = startDate.AddMinutes(60),
                                courtNum = randomCourt, //todo

                                CategoryId = input.CategoryId
                            };

                            // Add match to the match list and insert it into the Match table
                            matches.Add(match);

                            await _matchRepository.InsertAsync(match);
                            // await _unitOfWorkManager.Current.SaveChangesAsync();

                            //await _matchRepository.InsertAsync(newMatches[j]); - not suitable 
                        }
                        //await _unitOfWorkManager.Current.SaveChangesAsync();  //need to execute it, the later code is dependent on this added records
                        await _unitOfWorkManager.Current.SaveChangesAsync();
                        //no need use the manager, manager vs unit of work 
                        //manager manage multiple unitofwork, unitofwork - is a single transaction

                        // check round is previous, assign nextmatchid 
                        var previousRound = round - 1;

                        if (previousRound >= 1)
                        {
                            var previousMatches = await _matchRepository.GetListAsync(m => m.round == previousRound);

                            var currentMatches = await _matchRepository.GetListAsync(m => m.round == round);


                            //
                            if (previousMatches.Count >= currentMatches.Count * 2)
                            {

                                for (int q = 0; q < currentMatches.Count; q++)
                                {
                                    // previousMatches = [1,2,3,4,5,6,7,8]
                                    var twoMatches = previousMatches.Skip(q * 2).Take(2).ToList();
                                    twoMatches[0].nextMatchId = currentMatches[q].Id;
                                    twoMatches[1].nextMatchId = currentMatches[q].Id;

                                    await _matchRepository.UpdateAsync(twoMatches[0]); //whole thing no need dot ...
                                    await _matchRepository.UpdateAsync(twoMatches[1]);
                                }
                                await _unitOfWorkManager.Current.SaveChangesAsync();
                            }
                        }

                    } while (totalParticipants > 1);

                }
                else
                {
                    firstRoundMatch = randomizedParticipants.Count / 4;

                    do
                    {
                        firstRoundMatch = firstRoundMatch / 2;
                        round++;
                        for (int j = 0; j < firstRoundMatch; j++)
                        {
                            var match = new Match
                            {
                                round = round,
                                startTime = DateTime.Now,
                                endTime = DateTime.Now.AddMinutes(30),
                                courtNum = 1,
                                CategoryId = input.CategoryId
                            };

                            // Add match to the match list and insert it into the Match table
                            matches.Add(match);
                            await _matchRepository.InsertAsync(match);
                        }

                        await _unitOfWorkManager.Current.SaveChangesAsync();


                    } while (firstRoundMatch > 1);
                }


                //To assign participants to matches

                var matchParticipants = new List<MatchParticipant>();

                //Each match loop all randomize participants
                for (int matchIndex = 0; matchIndex < matches.Count; matchIndex++)
                {
                    var match = matches[matchIndex];

                    if (isSingle)
                    {
                        // Calculate the index to get the two participants for the current match
                        var participant1Index = matchIndex * 2;
                        var participant2Index = participant1Index + 1;

                        // Ensure indices are within the range of randomized participants
                        if (participant1Index < randomizedParticipants.Count && participant2Index < randomizedParticipants.Count)
                        {
                            var participant1 = randomizedParticipants[participant1Index];
                            var participant2 = randomizedParticipants[participant2Index];

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
                            await _matchParticipantRepository.InsertAsync(matchParticipants[matchIndex * 2]);
                            await _matchParticipantRepository.InsertAsync(matchParticipants[matchIndex * 2 + 1]);
                        }
                    }







                    //    for (int i = 0; i < matches.Count; i++)
                    //{
                    //    if (isSingle)
                    //    {
                    //        var participant1 = randomizedParticipants[i * 2]; //get [0], [2], [4]
                    //        var participant2 = randomizedParticipants[i * 2 + 1]; //get [1], [3], [5]

                    //        matchParticipants.Add(new MatchParticipant
                    //        {
                    //            MatchId = matches[i].Id, //match[0] 
                    //            UserId = participant1.UserId
                    //        });

                    //        matchParticipants.Add(new MatchParticipant
                    //        {
                    //            MatchId = matches[i].Id,
                    //            UserId = participant2.UserId
                    //        });
                    //        await _matchParticipantRepository.InsertAsync(matchParticipants[0]);
                    //        await _matchParticipantRepository.InsertAsync(matchParticipants[1]);
                    //    }

                    //}





                }
                return matchParticipants.GroupBy(mp => mp.MatchId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error:", ex.Message);
                return null;
            }
            }
            else
            {
                //throw exception
                throw new Exception($"Matches are already Generated. Each draw can only generate once!.");
            }

        }

        // Get participants for a specific match
        public async Task<List<MatchParticipantDto>> GetMatchParticipantsByMatchIdAsync(Guid matchId)
        {
            // Retrieve all participants for the specified matchId
            var matchParticipants = await _matchParticipantRepository.GetListAsync(mp => mp.MatchId == matchId);

            var userIds = matchParticipants.Select(mp => mp.UserId).Distinct().ToList();

            var users = await _userRepository.GetListByIdsAsync(userIds);
            var custUsers = await _customUserRepository.GetListAsync();

            var custUserFN = "";


            // Convert the entities into MatchParticipantDto
            var matchParticipantDtos = matchParticipants.Select(mp =>
            {
                var user = users.FirstOrDefault(u => u.Id == mp.UserId);
                
                var custUser = custUsers.FirstOrDefault(cu => cu.UserId == mp.UserId);
                custUserFN = custUser.FullName;

                return new MatchParticipantDto
                {
                    Id = mp.Id,
                    userId = mp.UserId,
                    pairId = mp.PairId,
                    matchId = mp.MatchId,
                    isWinner = mp.IsWinner,
                    userName = user != null ? user.UserName : "",
                    FullName = custUserFN,
                };
            }).ToList();

            return matchParticipantDtos;
        }



    }
}

