using Microsoft.VisualBasic;
using TodoItems.Core.ApplicationException;
using static TodoItems.Core.ConstantsAndEnums;

namespace TodoItems.Core
{
    public class TodoItemService
    {
        private readonly ITodosRepository _todosRepository;

        public TodoItemService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public TodoItem Create(string description, DateOnly? manualSetDueDate, DueDateSetStrategy strategy = DueDateSetStrategy.Manual)
        {
            var newItem = TodoItemFactory.CreateItem(_todosRepository, description, manualSetDueDate, strategy);
            _todosRepository.Create(newItem);
            return newItem;
        }

        public void Modify(TodoItem todoItem, string description)
        {
            todoItem.ModifyItem(description);
            _todosRepository.Save(todoItem);
        }
        
    }
}
