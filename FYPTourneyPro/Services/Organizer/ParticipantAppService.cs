using FYPTourneyPro.Entities.Organizer;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Volo.Abp.Domain.Repositories;
using FYPTourneyPro.Services.Dtos.Organizer;
using Volo.Abp.Identity;
using System.Collections.Generic;

namespace FYPTourneyPro.Services.Organizer
{
    public class ParticipantAppService : FYPTourneyProAppService
    {
        private readonly IRepository<Participant, Guid> _participantRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Registration, Guid> _registrationRepository;
        private readonly IIdentityUserRepository _userRepository;

        public ParticipantAppService(
            IRepository<Participant, Guid> participantRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Registration, Guid> registrationRepository,
            IIdentityUserRepository userRepository)
        {
            _participantRepository = participantRepository;
            _categoryRepository = categoryRepository;
            _registrationRepository = registrationRepository;
            _userRepository = userRepository;
        }

        // Create participant
        //public async Task<ParticipantDto> CreateAsync(Guid registrationId, Guid userId, Guid tournamentId)
        //{


        //    var participant = await _participantRepository.InsertAsync(new Participant
        //    {
        //        UserId = userId,
        //        RegistrationId = registrationId,
        //        TournamentId = tournamentId,
                
        //    });

        //    return new RegistrationDto
        //    {
        //        User = userId,
        //        RegistrationId = registrationId,
        //        TournamentId = tournamentId,
        //    };
        //}



        // Get participants by registrationId
        public async Task<List<ParticipantDto>> GetParticipantsByRegistrationAsync(Guid registrationId)
        {
            var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == registrationId);

            //loop participants, each part get registrationid, use regId to get registrationData, then get categoryId from registration data, when get catId, get the cat data, catId.catName

            List<ParticipantDto> participantListDto = new List <ParticipantDto>();

            foreach (var participant in participants)
            {
                var regId = participant.RegistrationId;

                // get registration 
                var registration =  await _registrationRepository.GetAsync(regId);
                var catId = registration.CategoryId;

                // get category
                var category = await _categoryRepository.GetAsync(catId);
                
                participantListDto.Add(new ParticipantDto
                {
                    Id = participant.Id,
                    UserId = participant.UserId,
                    RegistrationId = registrationId,
                    TournamentId = participant.TournamentId,
                    CategoryId = catId,
                    CategoryName = category.Name,
                    PairId = participant.PairId,
                    Points = participant.Points,
                    Seed = participant.Seed
                });
            }
            return participantListDto;
        }
    }
}