using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ToDoList.Api.Dto;

namespace ToDoList.Api.Services
{
    public class ToDoItemService : IToDoItemsService
    {
        private readonly IMongoCollection<ToDoItemDB> _ToDoItemsCollection;
        private readonly string _dataBaseName = "ToDoItems";

        public ToDoItemService(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase(_dataBaseName);
            _ToDoItemsCollection = mongoDatabase.GetCollection<ToDoItemDB>(_dataBaseName);
        }

        //var toDoItems = await _ToDoItemsCollection.Find(_ => true).ToListAsync();
        //var toDoItem = await _ToDoItemsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        //await _ToDoItemsCollection.InsertOneAsync(item);
        //await _ToDoItemsCollection.ReplaceOneAsync(x => x.Id == id, item);
        //var result = await _ToDoItemsCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<ToDoItemDto>> GetAsync()
        {
            var todoItemsDB = await _ToDoItemsCollection.Find(_ => true).ToListAsync();
            var todoItemsDto = todoItemsDB.Select(item => item.ConvertToDto()).ToList();
            return todoItemsDto;
        }

        public async Task<ToDoItemDto?> GetAsync(string id)
        {
            var itemDB = await _ToDoItemsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var itemDto = itemDB?.ConvertToDto();
            return itemDto;
        }

        public async Task CreateAsync(ToDoItemDto item)
        {
            await _ToDoItemsCollection.InsertOneAsync(item.ConvertToDB());
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _ToDoItemsCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
                return true;
            return false;
        }

        public async Task UpdateAsync(ToDoItemDto targetItem, ToDoItemDto updateItem)
        {
            await _ToDoItemsCollection.ReplaceOneAsync(x => x.Id == targetItem.Id, updateItem.ConvertToDB());
        }
    }
}
