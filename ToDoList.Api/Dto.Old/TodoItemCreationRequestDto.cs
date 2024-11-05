using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Dto
{
    public class TodoItemCreationRequestDto
    {
        [Required]
        [StringLength(50)]
        public required string Description { get; set; }
        public bool IsDone { get; set; } = false;
        public bool IsFavourite { get; set; } = false;
    }
}