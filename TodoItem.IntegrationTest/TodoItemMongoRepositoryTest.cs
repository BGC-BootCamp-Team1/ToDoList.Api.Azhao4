using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using TodoItem.Infrastructure;

namespace TodoItem.IntegrationTest;

public class TodoItemMongoRepositoryTest: IAsyncLifetime
{
    private readonly TodoItemMongoRepository _mongoRepository;
    private IMongoCollection<TodoItemPo> _mongoCollection;


    public TodoItemMongoRepositoryTest()
    {
        var mockSettings = new Mock<IOptions<TodoStoreDatabaseSettings>>();
        
        mockSettings.Setup(s => s.Value).Returns(new TodoStoreDatabaseSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "ToDoItems",
            TodoItemsCollectionName = "ToDoItems"
        });

        _mongoRepository = new TodoItemMongoRepository(mockSettings.Object);
        
        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("ToDoItems");
        _mongoCollection = mongoDatabase.GetCollection<TodoItemPo>("ToDoItems");
    }
    
    // IAsyncLifetime 中的 InitializeAsync 方法在每个测试前运行
    public async Task InitializeAsync()
    {
        await _mongoCollection.DeleteManyAsync(FilterDefinition<TodoItemPo>.Empty);
    }

    // DisposeAsync 在测试完成后运行（如果有需要的话）
    public Task DisposeAsync() => Task.CompletedTask;

    private TodoItems.Core.TodoItem _todoItem = new TodoItems.Core.TodoItem
    {
        Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
        Description = "test description",
        CreatedTime = DateTime.Now,
    };

[Fact]
    public async void Should_return_item_by_id()
    {
        var todoItemPo = new TodoItemPo{
            Id = "5f9a7d8e2d3b4a1eb8a7d8e2", 
            Description = "Buy groceries",
        };
        await _mongoCollection.InsertOneAsync(todoItemPo);
        var todoItem = await _mongoRepository.FindById("5f9a7d8e2d3b4a1eb8a7d8e2");
        
        Assert.NotNull(todoItem);
        Assert.Equal("5f9a7d8e2d3b4a1eb8a7d8e2", todoItem.Id);
        Assert.Equal("Buy groceries", todoItem.Description);
    }

    [Fact]
    public async void Should_create_item()
    {
        _mongoRepository.Create(_todoItem);

        var item = await _mongoRepository.FindById(_todoItem.Id);

        Assert.NotNull(item);
        Assert.Equal(_todoItem.Description, item.Description);
    }

    [Fact]
    public void Should_get_count_by_due_date()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var items = new List<TodoItemPo>
        {
            new() {
                Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
                Description = "test description",
                CreatedTime = DateTime.Now,
                DueDate = today.AddDays(1)
            },
            new()
            {
                Id = "5f9a7d8e2d3b4a1eb8a7d8e3",
                Description = "test description 2",
                CreatedTime = DateTime.Now,
                DueDate = today.AddDays(1)
            },
            new()
            {
                Id = "5f9a7d8e2d3b4a1eb8a7d8e4",
                Description = "test description 3",
                CreatedTime = DateTime.Now,
                DueDate = today.AddDays(2)
            },
            new()
            {
                Id = "5f9a7d8e2d3b4a1eb8a7d8e5",
                Description = "test description 4",
                CreatedTime = DateTime.Now,
                DueDate = today.AddDays(1)
            }
        };
        _mongoCollection.InsertMany(items);

        var count = _mongoRepository.GetCountByDueDate(today.AddDays(1));
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task Should_save_item_when_have_existing_id()
    {
        _mongoRepository.Create(_todoItem);
        var updateItem = new TodoItems.Core.TodoItem
        {
            Id = _todoItem.Id,
            Description = "test update",
            CreatedTime = _todoItem.CreatedTime,
            DueDate = _todoItem.DueDate,
            ModificationRecords = _todoItem.ModificationRecords
        };

        _mongoRepository.Save(updateItem);

        var resultItem = await _mongoRepository.FindById(updateItem.Id);
        Assert.Equal(updateItem.Description, resultItem.Description);
    }


    [Fact]
    public async Task Should_save_item_when_not_existing()
    {
        var updateItem = new TodoItems.Core.TodoItem
        {
            Id = _todoItem.Id,
            Description = "test update",
            CreatedTime = _todoItem.CreatedTime,
            DueDate = _todoItem.DueDate,
            ModificationRecords = _todoItem.ModificationRecords
        };

        _mongoRepository.Save(updateItem);

        var resultItem = await _mongoRepository.FindById(updateItem.Id);
        Assert.Equal(updateItem.Description, resultItem.Description);
    }
}