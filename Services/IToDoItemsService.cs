using ToDoList.Api.Dto;

namespace ToDoList.Api.Services
{
    public interface IToDoItemsService
    {
        Task CreateAsync(ToDoItemDto item);
        Task<bool> DeleteAsync(string id);
        Task<List<ToDoItemDto>> GetAsync();
        Task<ToDoItemDto?> GetAsync(string id);
        Task UpdateAsync(ToDoItemDto targetItem, ToDoItemDto updateItem);
    }
}