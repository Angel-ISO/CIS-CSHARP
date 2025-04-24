using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Entities;
public class Idea : BaseEntity
{
        [Required]
        public string? TopicId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }

        [Required]
        public string ? Description { get; set; }

        [Required]
        public string ? Username { get; set; }

        [Required]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonIgnore]
        public virtual Topic? Topic { get; set; }

        [BsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

}
