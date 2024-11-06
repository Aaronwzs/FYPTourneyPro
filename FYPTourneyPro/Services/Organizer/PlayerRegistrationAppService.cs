

using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer.PlayerRegistration;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class PlayerRegistrationAppService : FYPTourneyProAppService
    {
        private readonly IRepository<PlayerRegistration, Guid> _playerRegistrationRepository;


        public PlayerRegistrationAppService(IRepository<PlayerRegistration, Guid> PlayerRegistrationRepository)
        {
            _playerRegistrationRepository = PlayerRegistrationRepository;
        }

        public async Task<List<PlayerRegistrationDto>> GetListAsync()
        {
            var registrations = await _playerRegistrationRepository.GetListAsync();
            return registrations
                .Select(r => new PlayerRegistrationDto
                {
                    Id = r.Id,
                    Date = r.Date,
                    Amount = r.Amount,
                    CategoryId = r.CategoryId,
                    UserId = r.UserId,
                    TournamentId = r.TournamentId
                }).ToList();
        }
        public async Task<PlayerRegistrationDto> CreateAsync(PlayerRegistrationDto input)
        {
            var registration = await _playerRegistrationRepository.InsertAsync(new PlayerRegistration
            {
                Date = input.Date,
                Amount = input.Amount,
                CategoryId = input.CategoryId,
                UserId = input.UserId,
                TournamentId = input.TournamentId
            });

            return ObjectMapper.Map<PlayerRegistration, PlayerRegistrationDto>(registration);
        }

        public async Task<PlayerRegistrationDto> UpdateAsync(Guid id, PlayerRegistrationDto input)
        {
            var registration = await _playerRegistrationRepository.GetAsync(id);
            registration.Date = input.Date;
            registration.Amount = input.Amount;
            registration.CategoryId = input.CategoryId;
            registration.UserId = input.UserId;

            await _playerRegistrationRepository.UpdateAsync(registration);
            return ObjectMapper.Map<PlayerRegistration, PlayerRegistrationDto>(registration);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _playerRegistrationRepository.DeleteAsync(id);
        }
    }
}