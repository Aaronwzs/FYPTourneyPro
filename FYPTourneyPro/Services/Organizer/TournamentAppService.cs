

using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer.Tournament;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class TournamentAppService : FYPTourneyProAppService
    {
        private readonly IRepository<Tournament, Guid> _tournamentRepository;

        public TournamentAppService(IRepository<Tournament, Guid> tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public async Task<List<TournamentDto>> GetListAsync()
        {
            var tournaments = await _tournamentRepository.GetListAsync();
            return tournaments
                .Select(t => new TournamentDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    RegistrationStartDate = t.RegStartDate,
                    RegistrationEndDate = t.RegEndDate,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList();
        }

        public async Task<TournamentDto> GetAsync(Guid id)
        {
            var tournament = await _tournamentRepository.GetAsync(id);
            return ObjectMapper.Map<Tournament, TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateAsync(TournamentDto input)
        {
            var tournament = await _tournamentRepository.InsertAsync(new Tournament
            {
                Name = input.Name,
                Description = input.Description,
                RegStartDate = input.RegistrationStartDate,
                RegEndDate = input.RegistrationEndDate,
                StartDate = input.StartDate,
                EndDate = input.EndDate
            });

            return new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                RegistrationStartDate = tournament.RegStartDate,
                RegistrationEndDate = tournament.RegEndDate,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate
            };
        }
        public async Task<TournamentDto> UpdateAsync(Guid id, TournamentDto input)
        {
            var tournament = await _tournamentRepository.GetAsync(id);
            tournament.Name = input.Name;
            tournament.Description = input.Description;
            tournament.RegStartDate = input.RegistrationStartDate;
            tournament.RegEndDate = input.RegistrationEndDate;
            tournament.StartDate = input.StartDate;
            tournament.EndDate = input.EndDate;

            await _tournamentRepository.UpdateAsync(tournament);
            return new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                RegistrationStartDate = tournament.RegStartDate,
                RegistrationEndDate = tournament.RegEndDate,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate
            };
        }
        public async Task DeleteAsync(Guid id)
        {
            await _tournamentRepository.DeleteAsync(id);
        }
    }
  

}