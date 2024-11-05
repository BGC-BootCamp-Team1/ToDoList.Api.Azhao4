using static TodoItems.Core.ConstantsAndEnums;

namespace ToDoList.Api.Dto
{
    public class CreateTodoItemRequest
    {
        public required string Description { get; set; }
        public DateOnly? ManualSetDueDate { get; set; }
        public DueDateSetStrategy Strategy { get; set; }
    }
}
