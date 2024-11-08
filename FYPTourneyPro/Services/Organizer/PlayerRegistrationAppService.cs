

using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer.Category;
using FYPTourneyPro.Services.Dtos.Organizer.PlayerRegistration;
using Volo.Abp.Domain.Repositories;


namespace FYPTourneyPro.Services.Organizer
{
    public class PlayerRegistrationAppService : FYPTourneyProAppService
    {
        private readonly IRepository<PlayerRegistration, Guid> _playerRegistrationRepository;
       

        public PlayerRegistrationAppService(
            IRepository<PlayerRegistration, Guid> playerRegistrationRepository)
        {
            _playerRegistrationRepository = playerRegistrationRepository;
           
        }

       

        public async Task<List<PlayerRegDto>> GetListAsync()
        {
            var registrations = await _playerRegistrationRepository.GetListAsync();
            return registrations
                .Select(r => new PlayerRegDto
                {
                    Id = r.Id,
                    Date = r.Date,
                    Amount = r.Amount,
                    CategoryId = r.CategoryId,
                    UserId = r.UserId,
                    TournamentId = r.TournamentId
                }).ToList();
        }
        public async Task<PlayerRegDto> CreateAsync(PlayerRegDto input)
        {
            var registration = await _playerRegistrationRepository.InsertAsync(new PlayerRegistration
            {
                Date = input.Date,
                Amount = input.Amount,
                CategoryId = input.CategoryId,
                UserId = input.UserId,
                TournamentId = input.TournamentId,
                UserName = input.UserName
            });

            return new PlayerRegDto
            {
                Id = registration.Id,
                Amount = registration.Amount,
                CategoryId = registration.CategoryId,
                UserId = registration.UserId,
                TournamentId = registration.TournamentId,
                UserName = registration.UserName
            };
        }

        public async Task<PlayerRegDto> UpdateAsync(Guid id, PlayerRegDto input)
        {
            var registration = await _playerRegistrationRepository.GetAsync(id);
            registration.Date = input.Date;
            registration.Amount = input.Amount;
            registration.CategoryId = input.CategoryId;
            registration.UserId = input.UserId;
            registration.UserName = input.UserName;

            await _playerRegistrationRepository.UpdateAsync(registration);
           
            return new PlayerRegDto {
                Id = registration.Id, 
                Amount = registration.Amount,
                UserId = registration.UserId,
                TournamentId = registration.TournamentId,
                UserName= registration.UserName
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _playerRegistrationRepository.DeleteAsync(id);
        }


        public async Task<List<PlayerRegDto>> GetListByCategoryAsync(Guid categoryId)
        {
            var registrations = await _playerRegistrationRepository.GetListAsync(r => r.CategoryId == categoryId);
            return registrations.Select(r => new PlayerRegDto
            {
                Id = r.Id,
                Date = r.Date,
                Amount = r.Amount,
                CategoryId = r.CategoryId,
                UserId = r.UserId,
                TournamentId = r.TournamentId,

                UserName = r.UserName
            }).ToList();
        }

            public async Task<List<PlayerRegDto>> GetListByTournamentAsync(Guid tournamentId)
        {
            var registrations = await _playerRegistrationRepository.GetListAsync(r => r.TournamentId == tournamentId);
            return registrations
                .Select(r => new PlayerRegDto
                {
                    Id = r.Id,
                    Date = r.Date,
                    Amount = r.Amount,
                    CategoryId = r.CategoryId,
                    UserId = r.UserId,
                    TournamentId = r.TournamentId,

                    UserName = r.UserName
                }).ToList();
        }
    }
}