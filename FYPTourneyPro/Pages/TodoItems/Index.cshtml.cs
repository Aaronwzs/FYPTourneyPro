using FYPTourneyPro.Services.Dtos.TodoItems;
using FYPTourneyPro.Services.TodoItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FYPTourneyPro.Pages.TodoItems
{
    public class IndexModel : PageModel
    {
        public List<TodoItemDto> TodoItems { get; set; } = new();

        private readonly TodoAppService _todoAppService;

        public IndexModel(TodoAppService todoAppService)
        {
            _todoAppService = todoAppService;
        }

        public async Task OnGetAsync()
        {
            TodoItems = await _todoAppService.GetListAsync();
        }
    }
}
