using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;

public class Vote: BaseEntity
{
    [Required]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    [Required]
    [Range(-1, 1)]
    public int Value { get; set; }
    public DateTime VotedAt { get; set; } = DateTime.Now;

    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? IdeaId { get; set; }

    [BsonIgnore]
    public virtual Idea? Idea { get; set; }
}