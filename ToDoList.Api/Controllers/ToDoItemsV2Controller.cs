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
        private ITodoItemService _service;

        public ToDoItemsV2Controller(ILogger<ToDoItemsController> logger, ITodoItemService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost()]
        public ActionResult<ToDoItemDto> Create([FromBody] CreateTodoItemRequest request)
        {
            var item = _service.Create(request.Description, request.ManualSetDueDate, request.Strategy);
            return Created(string.Empty, item);
        }

        [HttpPut("{id}")]
        public ActionResult<ToDoItemDto> Update(string id, [FromBody] ToDoItemDto todoItemDto)
        {
            _service.ModifyDescription(id, todoItemDto.Description);
            return Ok();
        }
    }

}
