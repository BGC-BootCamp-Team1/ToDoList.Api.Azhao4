using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Driver;
using System.Net;
using System.Text;
using System.Text.Json;
using TodoItem.Infrastructure;
using ToDoList.Api.Dto;
using static TodoItems.Core.ConstantsAndEnums;

namespace ToDoList.Api.Test;
public class ToDoItemV2ControllerTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private IMongoCollection<TodoItemPo> _mongoCollection;

    public ToDoItemV2ControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var mongoClient = new MongoClient("mongodb://localhost:27017");
        var mongoDatabase = mongoClient.GetDatabase("ToDoItems");
        _mongoCollection = mongoDatabase.GetCollection<TodoItemPo>("ToDoItems");
    }

    public async Task InitializeAsync()
    {
        await _mongoCollection.DeleteManyAsync(FilterDefinition<TodoItemPo>.Empty);
    }

    public Task DisposeAsync() => Task.CompletedTask;


    [Fact]
    public async Task PostAsync_CreateToDoItem_MustReturnCreatedResult()
    {
        // Arrange
        var request = new CreateTodoItemRequest
        {
            Description = "Test create",
            ManualSetDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
            Strategy = DueDateSetStrategy.Manual
        };

        var json = JsonSerializer.Serialize(request);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v2/todoitemsv2", requestContent);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();

        var createdTodo = JsonSerializer.Deserialize<TodoItemPo>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(createdTodo);
        Assert.Equal(request.Description, createdTodo.Description);
        Assert.Equal(request.ManualSetDueDate, createdTodo.DueDate);

    }


    [Fact]
    public async Task PutAsync_ModifyDescription_MustReturnItemResult()
    {
        var todoItemPo = new TodoItemPo
        {
            Description = "old description",
            CreatedTime = DateTime.Now,
            Id = "fa88103e-35bb-4b74-8ca8-075c6093eef0",
        };
        await _mongoCollection.InsertOneAsync(todoItemPo);


        var todoItem = new ToDoItemDto
        {
            Description = "old description",
            CreatedTime = DateTime.Now,
            Id = "fa88103e-35bb-4b74-8ca8-075c6093eef0",
        };
        var json = JsonSerializer.Serialize(todoItem);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync("/api/v2/todoitemsv2/fa88103e-35bb-4b74-8ca8-075c6093eef0", requestContent);


        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
