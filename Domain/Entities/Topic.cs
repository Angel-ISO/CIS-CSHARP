using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class Topic: BaseEntity
{
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
        public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
}
