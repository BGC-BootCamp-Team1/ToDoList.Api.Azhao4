using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoItems.Core;
using ToDoList.Api.Dto;

namespace ToDoList.Api.Test
{
    public class GetOneTodoItemTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private IMongoCollection<ToDoItemDB> _mongoCollection;

        public GetOneTodoItemTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();

            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var mongoDatabase = mongoClient.GetDatabase("ToDoItems");
            _mongoCollection = mongoDatabase.GetCollection<ToDoItemDB>("ToDoItems");
        }

        public async Task InitializeAsync()
        {
            await _mongoCollection.DeleteManyAsync(FilterDefinition<ToDoItemDB>.Empty);
        }

        public Task DisposeAsync() => Task.CompletedTask;


        [Fact]
        public async void Should_get_todo_by_given_id()
        {
            // Arrange
            var todoItem = new ToDoItemDB
            {
                Id = "5f9a7d8e2d3b4a1eb8a7d8e2",
                Description = "Buy groceries",
                IsDone = false,
                IsFavourite = true
            };

            await _mongoCollection.InsertOneAsync(todoItem);

            // Act
            var response = await _client.GetAsync("/api/v1/todoitems/5f9a7d8e2d3b4a1eb8a7d8e2");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            var returnedTodos = JsonSerializer.Deserialize<ToDoItemDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(returnedTodos);
            Assert.Equal("Buy groceries", returnedTodos.Description);
            Assert.True(returnedTodos.IsFavourite);
            Assert.False(returnedTodos.IsDone);
        }

        [Fact]
        public async void should_get_NOTFOUND_by_invalid_id()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/api/v1/todoitems/not_exist_id");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
