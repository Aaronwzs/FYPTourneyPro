

using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Services.Dtos.Organizer;
using System.Linq;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace FYPTourneyPro.Services.Organizer
{
    public class TournamentAppService : FYPTourneyProAppService
    {
        private readonly IRepository<Tournament, Guid> _tournamentRepository;
        private readonly ICurrentUser _currentUser;

        public TournamentAppService(IRepository<Tournament, Guid> tournamentRepository, ICurrentUser currentUser)
        {
            _tournamentRepository = tournamentRepository;
            _currentUser = currentUser;
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

        public async Task<List<TournamentDto>> GetListAsyncUid()
        {
            // Fetch tournaments created by the current logged-in user
            var tournaments = await _tournamentRepository.GetListAsync(t => t.UserId == _currentUser.Id); //filter by userid


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
            if (tournament.UserId != _currentUser.Id)
            {
                throw new Exception("You are not authorized to view this tournament.");
            }
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

        public async Task<TournamentDto> CreateAsync(TournamentDto input)
        {
            if (_currentUser.IsAuthenticated)
            {
                if (input.RegistrationStartDate > input.RegistrationEndDate)
                {
                    throw new Exception("Registration start date cannot be later than registration end date.");
                }

                // Validation for Start Date and Registration End Date
                if (input.StartDate <= input.RegistrationEndDate)
                {
                    throw new Exception("Start date must be later than the registration end date.");
                }

                // Validation for End Date and Start Date
                if (input.EndDate <= input.StartDate)
                {
                    throw new Exception("End date must be later than the start date.");
                }

                var tournament = await _tournamentRepository.InsertAsync(new Tournament
                {
                    Name = input.Name,
                    Description = input.Description,
                    RegStartDate = input.RegistrationStartDate,
                    RegEndDate = input.RegistrationEndDate,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    UserId = _currentUser.Id.Value
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
            else
            {
                throw new Exception("User must be logged in to create a tournament.");
            }
        }
            public async Task<TournamentDto> UpdateAsync(Guid id, TournamentDto input)
        {

            if (input.RegistrationStartDate > input.RegistrationEndDate)
            {
                throw new Exception("Registration start date cannot be later than registration end date.");
            }

            // Validation for Start Date and Registration End Date
            if (input.StartDate <= input.RegistrationEndDate)
            {
                throw new Exception("Start date must be later than the registration end date.");
            }

            // Validation for End Date and Start Date
            if (input.EndDate <= input.StartDate)
            {
                throw new Exception("End date must be later than the start date.");
            }

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