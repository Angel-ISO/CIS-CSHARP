using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Domain.Entities;
public class Vote : BaseEntity
{
        [Required]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }


        [Required]
        [Range(-1, 1)]
        public int Value { get; set; } 

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;


        [Required]
        public string? IdeaId { get; set; }

        [BsonIgnore]
        public virtual Idea? Idea { get; set; }
}
