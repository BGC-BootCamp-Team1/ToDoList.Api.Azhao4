using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Dto;
using ToDoList.Api.Services;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ToDoItemsController : ControllerBase
    {
        private readonly ILogger<ToDoItemsController> _logger;
        private IToDoItemsService _service;

        public ToDoItemsController(ILogger<ToDoItemsController> logger, IToDoItemsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet()]
        public async Task<ActionResult<List<ToDoItemDto>>> Get()
        {
            var items = await _service.GetAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemDto>> Get(string id)
        {
            var item = await _service.GetAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost()]
        public async Task<ActionResult<ToDoItemDto>> Create(TodoItemCreationRequestDto creationRequest)
        {
            var newItem = new ToDoItemDto()
            {
                Id = Guid.NewGuid().ToString(),
                Description = creationRequest.Description,
                IsDone = creationRequest.IsDone,
                IsFavourite = creationRequest.IsFavourite,
            };
            await _service.CreateAsync(newItem);
            return Created(string.Empty, newItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var targetItem = await _service.GetAsync(id);
            if (targetItem == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDoItemDto>> Update(string id, ToDoItemDto updateItem)
        {
            if (id != updateItem.Id)
                return BadRequest("ToDo Item ID in url must be equal to request body");

            var targetItem = await _service.GetAsync(id);
            if (targetItem == null)
            {
                await _service.CreateAsync(updateItem);
                return Created("", updateItem);
            }
            else
            {
                await _service.UpdateAsync(targetItem, updateItem);
                return Ok(targetItem);
            }
        }
    }

}
