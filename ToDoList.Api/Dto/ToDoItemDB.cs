using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Dto
{
    [BsonIgnoreExtraElements]
    public class ToDoItemDB
    {
        [BsonId]
        public required string Id { get; init; }

        [Required]
        [StringLength(50)]
        public required string Description { get; set; }
        public bool IsDone { get; set; } = false;
        public bool IsFavourite { get; set; } = false;

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;

        public ToDoItemDto ConvertToDto()
        {
            return new ToDoItemDto()
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
