using FYPTourneyPro.Entities.Organizer;
using FYPTourneyPro.Entities.TodoList;
using FYPTourneyPro.Services.Dtos.Organizer.Category;
using FYPTourneyPro.Services.Dtos.TodoItems;
using OpenQA.Selenium;
using SendGrid.Helpers.Errors.Model;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.Organizer
{
    public class CategoryAppService : FYPTourneyProAppService
    {

        private readonly IRepository<Category, Guid> _categoryRepository;


        public CategoryAppService(IRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetListAsync()
        {
            var categories = await _categoryRepository.GetListAsync();
            return categories
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,


                }).ToList();
        }
        public async Task<CategoryDto> CreateAsync(CategoryDto input)
        {
            var category = new Category
            {
                Name = input.Name,
                Description = input.Description,
                TournamentId = input.TournamentId
            };

            var createdCategory = await _categoryRepository.InsertAsync(category);

            return new CategoryDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                TournamentId = createdCategory.TournamentId
            };
        }

        public async Task<CategoryDto> EditAsync(Guid id, string name, string description)
        {
            var existingCategory = await _categoryRepository.FindAsync(id);
            if (existingCategory == null)
            {
                throw new OpenQA.Selenium.NotFoundException($"Category with ID {id} not found.");
            }

            existingCategory.Name = name;
            existingCategory.Description = description;

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
            return new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Description = updatedCategory.Description,
                TournamentId = updatedCategory.TournamentId
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
        
        
        public async Task<List<CategoryDto>> GetListByTournamentAsync(Guid tournamentId)
{
    var categories = await _categoryRepository.GetListAsync(c => c.TournamentId == tournamentId);
    return categories
        .Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            TournamentId = category.TournamentId
        }).ToList();
}

        public async Task<CategoryDto> GetAsync(Guid id)
        {
            var category = await _categoryRepository.FindAsync(id);
            if (category == null)
            {
                throw new EntityNotFoundException(typeof(Category), id);
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                TournamentId = category.TournamentId
            };
        }

    }
}
