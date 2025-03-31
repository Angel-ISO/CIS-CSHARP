using System.ComponentModel.DataAnnotations;

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
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Idea> Ideas { get; set; } = new List<Idea>();
}
