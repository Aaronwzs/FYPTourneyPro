using FYPTourneyPro.Entities.TodoList;
using FYPTourneyPro.Permissions;
using FYPTourneyPro.Services.Dtos.TodoItems;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;

namespace FYPTourneyPro.Services.TodoItems
{
    public class TodoAppService : FYPTourneyProAppService
    {
        private readonly IRepository<TodoItem, Guid> _todoItemRepository;
        private readonly IAuthorizationService _authorizationService;

        public TodoAppService(IRepository<TodoItem, Guid> todoItemRepository,
             IAuthorizationService authorizationService)
        {
            _todoItemRepository = todoItemRepository;
            _authorizationService = authorizationService;
        }

        public async Task<List<TodoItemDto>> GetListAsync()
        {
            var items = await _todoItemRepository.GetListAsync();
            return items
                .Select(item => new TodoItemDto
                {
                    Id = item.Id,
                    Text = item.Text
                }).ToList();
        }

        public async Task<TodoItemDto> CreateAsync(string text)
        {
            //check if user has permission to create a todoItem
            var isAuthorized = await _authorizationService.IsGrantedAsync(FYPTourneyProPermissions.TodoItems.Create);
            if (isAuthorized)
            {
                var todoItem = await _todoItemRepository.InsertAsync(
               new TodoItem { Text = text }
           );

                return new TodoItemDto
                {
                    Id = todoItem.Id,
                    Text = todoItem.Text
                };

            }
            else
            {
                throw new AbpAuthorizationException($"You are not authorized to create todo items. Required permission: {FYPTourneyProPermissions.TodoItems.Create}");
            }
           
        }

        public async Task DeleteAsync(Guid id)
        {
            // Check if the user has permission to delete a todo item
            var isAuthorized = await _authorizationService.IsGrantedAsync(FYPTourneyProPermissions.TodoItems.Delete);
            if (!isAuthorized)
            {
                throw new AbpAuthorizationException($"You are not authorized to create todo items. Required permission: {FYPTourneyProPermissions.TodoItems.Delete}");
            }
            await _todoItemRepository.DeleteAsync(id);
        }
    }
}
