using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Dto;

namespace ToDoList.Api.Services
{
    public class InMemoryToDoItemsService : IToDoItemsService
    {
        private readonly List<ToDoItemDto> _items = new List<ToDoItemDto>();

        [HttpGet()]
        public async Task<List<ToDoItemDto>> GetAsync()
        {
            return _items;
        }

        public async Task<ToDoItemDto?> GetAsync(string id)
        {
            var result = _items.Find(i => i.Id.Equals(id));
            return result;
        }

        public async Task CreateAsync(ToDoItemDto item)
        {
            _items.Add(item);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = await GetAsync(id);
            if (item == null)
                return false;

            _items.Remove(item);
            return true;
        }

        public async Task UpdateAsync(ToDoItemDto targetItem, ToDoItemDto updateItem)
        {
            targetItem.Description = updateItem.Description;
            targetItem.IsDone = updateItem.IsDone;
            targetItem.IsFavourite = updateItem.IsFavourite;
        }
    }
}
