namespace TodoItems.Core
{
    public interface ITodosRepository
    {
        void Create(TodoItem item);
        int GetCountByDueDate(DateOnly date);
        void Save(TodoItem todoItem);
        TodoItem? FindById(string? id);
    }
}
