using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using Volo.Abp.Domain.Repositories;
using AutoMapper.Internal.Mappers;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;
using static Volo.Abp.Identity.IdentityPermissions;
using Volo.Abp.Identity;

namespace FYPTourneyPro.Services.Organizer
{
    public class RegistrationAppService : ApplicationService
    {
        private readonly IRepository<Registration, Guid> _registrationRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Participant, Guid> _participantRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _userRepository;


        public RegistrationAppService(
            IRepository<Registration, Guid> registrationRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Participant, Guid> participantRepository, ICurrentUser currentUser, IIdentityUserRepository userRepository)
        {
            _registrationRepository = registrationRepository;
            _categoryRepository = categoryRepository;
            _participantRepository = participantRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
        }

        // Create a registration and add participants
        public async Task<RegistrationDto> CreateAsync(RegistrationDto input)
        {
            var registration = new Registration
            {
                CategoryId = input.CategoryId,
                RegDate = DateTime.Now,
                totalAmount = input.TotalAmount
            };

            var createdRegistration = await _registrationRepository.InsertAsync(registration);
            
            var pairId = Guid.NewGuid();
            var user1 = await _userRepository.GetListAsync(userName: input.userName1);
            var user2 = await _userRepository.GetListAsync(userName: input.userName2);
            var userId1 = user1[0].Id;
            var userId2 = user2[0].Id;

            var category = await _categoryRepository.GetAsync(input.CategoryId);

            // Create participants for userId1 and userId2
            if (category.isPair)  // If pair exists
            {
                
                await CreateParticipantAsync(userId1, createdRegistration.Id, input.tournamentId, pairId);
                await CreateParticipantAsync(userId2, createdRegistration.Id, input.tournamentId, pairId);
            }
            else  // Single user (No pair)
            {
                await CreateParticipantAsync(userId1, createdRegistration.Id, input.tournamentId, pairId);
            }

            return new RegistrationDto
            {
                Id = createdRegistration.Id,
                CategoryId = createdRegistration.CategoryId,
                RegDate = createdRegistration.RegDate,
                TotalAmount = createdRegistration.totalAmount
            };
        }

        // Helper method to create a participant
        private async Task CreateParticipantAsync(Guid userId, Guid registrationId, Guid tournamentId, Guid pairId)
        {
            var participant = new Participant
            {
                UserId = userId,
                RegistrationId = registrationId,
                TournamentId = tournamentId,
                PairId = pairId,
                Seed = 0,
                Points = 0
            };

            await _participantRepository.InsertAsync(participant);
        }

        // Get the list of registrations by tournament ID
        public async Task<List<RegistrationDto>> GetListByTournamentAsync(Guid tournamentId)
        {
            var registrations = await _registrationRepository.GetListAsync(r => r.CategoryId == tournamentId);

            var registrationDtos = new List<RegistrationDto>();
            foreach (var registration in registrations)
            {
                var category = await _categoryRepository.GetAsync(registration.CategoryId);

                registrationDtos.Add(new RegistrationDto
                {
                    Id = registration.Id,
                    CategoryId = category.Id,
                    
                    RegDate = registration.RegDate,
                    TotalAmount = registration.totalAmount
                });
            }

            return registrationDtos;
        }

        // Delete a registration (and associated participants)
        public async Task DeleteAsync(Guid registrationId)
        {
            var registration = await _registrationRepository.GetAsync(registrationId);
            await _registrationRepository.DeleteAsync(registration);

            // Delete participants associated with this registration
            var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == registrationId);
            foreach (var participant in participants)
            {
                await _participantRepository.DeleteAsync(participant);
            }
        }

        // Fetch participants for a given registration
        public async Task<List<ParticipantDto>> GetParticipantsAsync(Guid registrationId)
        {
            var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == registrationId);
            var participantDtos = new List<ParticipantDto>();

            foreach (var participant in participants)
            {
                participantDtos.Add(new ParticipantDto
                {
                    Id = participant.Id,
                    UserId = participant.UserId,
                    RegistrationId = participant.RegistrationId,
                    TournamentId = participant.TournamentId,
                    CategoryId = participant.RegistrationId,  // CategoryId is stored in registration
                    PairId = participant.PairId ?? null,
                    Seed = participant.Seed,
                    Points = participant.Points
                });
            }

            return participantDtos;
        }

        public async Task<List<RegistrationDto>> GetRegistrationListAsync(Guid registrationId)
        {
            var registrations = await _registrationRepository.GetListAsync(p => p.Id == registrationId);
            

            //loop participants, each part get registrationid, use regId to get registrationData, then get categoryId from registration data, when get catId, get the cat data, catId.catName

            List<RegistrationDto> RegistrationListDto = new List<RegistrationDto>();

            //check if response is empty then return the empty list
            if (registrations.Count == 0)
            {
                return RegistrationListDto;
            }

            foreach (var reg in registrations)
            {
                // get participant
                var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == registrationId);

                
                IdentityUser? user1 = null;
                IdentityUser? user2 = null;

                if (participants[0].PairId != null)
                {
                    user1 = await _userRepository.GetAsync(participants[0].UserId);
                    user1 = await _userRepository.GetAsync(participants[1].UserId);
                } else
                {
                    user1 = await _userRepository.GetAsync(participants[0].UserId);
                }

                var category = await _categoryRepository.GetAsync(reg.CategoryId);

                RegistrationListDto.Add(new RegistrationDto
                {
                    Id = reg.Id,
                    RegDate = reg.RegDate,
                    TotalAmount = reg.totalAmount,
                    userId1 = user1.Id,
                    userId2 = user2 != null ? user2.Id : null,
                    userName1 = user1.UserName,
                    userName2 = user2 != null ? user2.UserName : null,
                    CategoryId = reg.CategoryId,
                    CategoryName = category.Name
                });
            }
            return RegistrationListDto;
        }

        public async Task<List<RegistrationDto>> GetRegistrationListByCategoryAsync(Guid CategoryId) //When click on Category populate the registration
        {
            var registrations = await _registrationRepository.GetListAsync(p => p.CategoryId == CategoryId);


            //loop participants, each part get registrationid, use regId to get registrationData, then get categoryId from registration data, when get catId, get the cat data, catId.catName

            List<RegistrationDto> RegistrationListDto = new List<RegistrationDto>();

            

            //check if response is empty then return the empty list
            if (registrations.Count == 0)
            {
                return RegistrationListDto;
            }

            foreach (var reg in registrations)
            {
                var regId = reg.Id;
                // get participant
                var participants = await _participantRepository.GetListAsync(p => p.RegistrationId == regId);
                IdentityUser? user1 = null;
                IdentityUser? user2 = null;

                //if (participants[0].PairId != null)
                //{
                //    user1 = await _userRepository.GetAsync(participants[0].UserId);
                //    user1 = await _userRepository.GetAsync(participants[1].UserId);
                //}
                //else
                //{
                //    user1 = await _userRepository.GetAsync(participants[0].UserId);
                //}


                // Get the first participant
                if (participants.Count > 0)
                {
                    user1 = await _userRepository.GetAsync(participants[0].UserId);
                }

                // Get the second participant if it exists
                if (participants.Count > 1)
                {
                    user2 = await _userRepository.GetAsync(participants[1].UserId);
                }


                var category = await _categoryRepository.GetAsync(reg.CategoryId);

                RegistrationListDto.Add(new RegistrationDto
                {
                    Id = reg.Id,
                    RegDate = reg.RegDate,
                    TotalAmount = reg.totalAmount,
                    userId1 = user1.Id,
                    userId2 = user2 != null ? user2.Id : null,
                    userName1 = user1.UserName,
                    userName2 = user2 != null ? user2.UserName : null,
                    CategoryId = reg.CategoryId,
                    CategoryName = category.Name
                    
                });
            }
            return RegistrationListDto;
        }
    }
}