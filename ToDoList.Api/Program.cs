using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoItem.Infrastructure;
using TodoItems.Core;
using ToDoList.Api.Dto;
using ToDoList.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ToDoItemDatabaseSettings>(builder.Configuration.GetSection("ToDoItemDatabase"));
builder.Services.AddSingleton<IToDoItemsService, ToDoItemService>();

builder.Services.Configure<TodoStoreDatabaseSettings>(builder.Configuration.GetSection("ToDoItemDatabase"));

builder.Services.AddSingleton<ITodosRepository, TodoItemMongoRepository>();
builder.Services.AddSingleton<ITodoItemService, TodoItemService>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ToDoItemDatabaseSettings>>();
    return new MongoClient(settings.Value.ConnectionString);
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();

public partial class Program { }