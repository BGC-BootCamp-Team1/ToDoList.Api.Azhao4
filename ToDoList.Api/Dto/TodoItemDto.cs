using System.ComponentModel.DataAnnotations;
using TodoItems.Core;

namespace ToDoList.Api.Dto
{
    public class ToDoItemDto
    {
        public required string Id { get; init; }

        [Required]
        [StringLength(50)]
        public required string Description { get; set; }
        public bool IsDone { get; set; } = false;
        public bool IsFavourite { get; set; } = false;

        public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;

        public DateOnly? DueDate { get; set; }
        public List<Modification> ModificationRecords { get; set; } = [];

        public ToDoItemDB ConvertToDB()
        {
            return new ToDoItemDB()
            {
                Id = Id,
                Description = Description,
                IsDone = IsDone,
                IsFavourite = IsFavourite,
                CreatedTime = CreatedTime,
            };
        }
    }
}
