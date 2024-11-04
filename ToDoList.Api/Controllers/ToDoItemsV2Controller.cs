using Microsoft.AspNetCore.Mvc;
using TodoItems.Core;
using ToDoList.Api.Dto;
using ToDoList.Api.Services;
using static TodoItems.Core.ConstantsAndEnums;

namespace ToDoList.Api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class ToDoItemsV2Controller : ControllerBase
    {
        private readonly ILogger<ToDoItemsController> _logger;
        private TodoItemService _service;

        public ToDoItemsV2Controller(ILogger<ToDoItemsController> logger, TodoItemService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost()]
        public ActionResult<ToDoItemDto> Create(string description, DateOnly? manualSetDueDate, DueDateSetStrategy strategy = DueDateSetStrategy.Manual)
        {
            var item = _service.Create(description, manualSetDueDate, strategy);
            return Created(string.Empty, item);
        }

        [HttpPut("{id}")]
        public ActionResult<ToDoItemDto> Update(string id, ToDoItemDto updateItem)
        {
            if (id != updateItem.Id)
                return BadRequest("ToDo Item ID in url must be equal to request body");
            return Ok();
            //var targetItem = await _service.GetAsync(id);
            //if (targetItem == null)
            //{
            //    await _service.CreateAsync(updateItem);
            //    return Created("", updateItem);
            //}
            //else
            //{
            //    await _service.UpdateAsync(targetItem, updateItem);
            //    return Ok(targetItem);
            //}
        }
    }

}
