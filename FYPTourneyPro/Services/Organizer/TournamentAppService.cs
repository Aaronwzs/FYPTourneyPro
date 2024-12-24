

using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Permissions;
using FYPTourneyPro.Services.Dtos.Organizer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthorizationService _authorizationService;


        public TournamentAppService(IRepository<Tournament, Guid> tournamentRepository, ICurrentUser currentUser, IAuthorizationService authorizationService)
        {
            _tournamentRepository = tournamentRepository;
            _currentUser = currentUser;
            _authorizationService = authorizationService;
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
                    EndDate = t.EndDate,
                    HotlineNum = t.HotlineNum,
                    isMalaysian = t.IsMalaysian, 
                    RulesAndRegulations = t.RulesAndRegulations  
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
                    EndDate = t.EndDate,
                    HotlineNum = t.HotlineNum,
                    isMalaysian = t.IsMalaysian,
                    RulesAndRegulations = t.RulesAndRegulations
                }).ToList();
        }

        public async Task<TournamentDto> GetAsync(Guid id)
        {
            var tournament = await _tournamentRepository.GetAsync(id);
            //if (tournament.UserId != _currentUser.Id)
            //{
            //    throw new Exception("You are not authorized to view this tournament.");
            //}
            return new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                RegistrationStartDate = tournament.RegStartDate,
                RegistrationEndDate = tournament.RegEndDate,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                HotlineNum = tournament.HotlineNum,
                isMalaysian = tournament.IsMalaysian,
                RulesAndRegulations = tournament.RulesAndRegulations
            };
        }

        public async Task<TournamentDto> CreateAsync(TournamentDto input)
        {
            //check if user has permission to create a todoItem
            var isAuthorized = await _authorizationService.IsGrantedAsync(FYPTourneyProPermissions.Tournaments.Create);
            if (isAuthorized)
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
                        UserId = _currentUser.Id.Value,
                        HotlineNum = input.HotlineNum,
                        RulesAndRegulations = input.RulesAndRegulations,
                        IsMalaysian = input.isMalaysian
                    });

                    return new TournamentDto
                    {
                        Id = tournament.Id,
                        Name = tournament.Name,
                        Description = tournament.Description,
                        RegistrationStartDate = tournament.RegStartDate,
                        RegistrationEndDate = tournament.RegEndDate,
                        StartDate = tournament.StartDate,
                        EndDate = tournament.EndDate,
                        HotlineNum = tournament.HotlineNum,
                        isMalaysian = tournament.IsMalaysian,
                        RulesAndRegulations = tournament.RulesAndRegulations
                    };

                }
                else
                {
                    throw new Exception("User must be logged in to create a tournament.");
                }
            }
            else
            {
                throw new AbpAuthorizationException($"You are not authorized to create Tournaments . Required permission: {FYPTourneyProPermissions.Tournaments.Create}");
            }
        }
            public async Task<TournamentDto> UpdateAsync(Guid id, TournamentDto input)
        {

            // Check if the user has permission to delete a todo item
            var isAuthorized = await _authorizationService.IsGrantedAsync(FYPTourneyProPermissions.Tournaments.Edit);
            if (!isAuthorized)
            {
                throw new AbpAuthorizationException($"You are not authorized to update tournaments. Required permission: {FYPTourneyProPermissions.Tournaments.Edit}");
            }

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
             tournament.HotlineNum = input.HotlineNum;  
            tournament.IsMalaysian = input.isMalaysian;  
            tournament.RulesAndRegulations = input.RulesAndRegulations; 

            await _tournamentRepository.UpdateAsync(tournament);
            return new TournamentDto
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                RegistrationStartDate = tournament.RegStartDate,
                RegistrationEndDate = tournament.RegEndDate,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                HotlineNum = tournament.HotlineNum,  
                isMalaysian = tournament.IsMalaysian,  
                RulesAndRegulations = tournament.RulesAndRegulations
            };
        }
        public async Task DeleteAsync(Guid id)
        {

            // Check if the user has permission to delete a todo item
            var isAuthorized = await _authorizationService.IsGrantedAsync(FYPTourneyProPermissions.Tournaments.Delete);
            if (!isAuthorized)
            {
                throw new AbpAuthorizationException($"You are not authorized to create todo items. Required permission: {FYPTourneyProPermissions.Tournaments.Delete}");
            }
            await _tournamentRepository.DeleteAsync(id);
        }
    }

  

}