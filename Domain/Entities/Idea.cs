using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities;
public class Idea : BaseEntity
{
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ?TopicId { get; set; }

    public string? Title { get; set; }

    [Required]
    public string? Content { get; set; } 

    [Required]
    public string? Username { get; set; }

    [Required]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [BsonIgnore]
    public virtual Topic? Topic { get; set; } 
    
    [BsonIgnore]
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

}
