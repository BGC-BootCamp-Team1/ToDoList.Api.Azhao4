using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItems.Core;

namespace TodoItem.Infrastructure;

public class TodoItemMongoRepository: ITodosRepository
{
    private readonly IMongoCollection<TodoItemPo?> _todosCollection;
    
    public TodoItemMongoRepository(IOptions<TodoStoreDatabaseSettings> todoStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(todoStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(todoStoreDatabaseSettings.Value.DatabaseName);
        _todosCollection = mongoDatabase.GetCollection<TodoItemPo>(todoStoreDatabaseSettings.Value.TodoItemsCollectionName);
    }

    public TodoItems.Core.TodoItem? FindById(string? id)
    {
        FilterDefinition<TodoItemPo?> filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, id);
        TodoItemPo? todoItemPo = _todosCollection.Find(filter).FirstOrDefault();

        TodoItems.Core.TodoItem todoItem = ConvertToTodoItem(todoItemPo);
        return todoItem;
    }

    public void Save(TodoItems.Core.TodoItem todoItem)
    {
        var filter = Builders<TodoItemPo>.Filter.Eq(x => x.Id, todoItem.Id);
        var update = Builders<TodoItemPo>.Update
            .Set(x => x.Description, todoItem.Description)
            .Set(x => x.DueDate, todoItem.DueDate)
            .Set(x => x.ModificationRecords, todoItem.ModificationRecords);

        var options = new UpdateOptions { IsUpsert = true };

        _todosCollection.UpdateOne(filter, update, options);
    }

    public void Create(TodoItems.Core.TodoItem item)
    {
        _todosCollection.InsertOne(ConvertFromTodoItem(item));
    }

    public int GetCountByDueDate(DateOnly date)
    {
        var query = _todosCollection.Find(itemPo => itemPo.DueDate == date);
        return query.ToList().Count;
    }

    private TodoItems.Core.TodoItem? ConvertToTodoItem(TodoItemPo? todoItemPo)
    {
        if (todoItemPo == null) return null;

        return new TodoItems.Core.TodoItem()
        {
            Id = todoItemPo.Id,
            Description = todoItemPo.Description,
            CreatedTime = todoItemPo.CreatedTime,
            DueDate = todoItemPo.DueDate,
            ModificationRecords = todoItemPo.ModificationRecords
        };
    }

    private TodoItemPo ConvertFromTodoItem(TodoItems.Core.TodoItem todoItem)
    {
        return new TodoItemPo
        {
            Id = todoItem.Id,
            Description = todoItem.Description,
            CreatedTime = todoItem.CreatedTime,
            DueDate = todoItem.DueDate,
            ModificationRecords = todoItem.ModificationRecords
        };
    }
}
