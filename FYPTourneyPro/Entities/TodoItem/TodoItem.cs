using Volo.Abp.Domain.Entities;

namespace FYPTourneyPro.Entities.TodoList
{
    public class TodoItem: BasicAggregateRoot<Guid>
    {
        public string Text { get; set; } = string.Empty;
    }
}
