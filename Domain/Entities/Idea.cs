using System.ComponentModel.DataAnnotations;


namespace Domain.Entities;
public class Idea : BaseEntity
{
        [Required]
        public Guid TopicId { get; set; }

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

        public virtual Topic? Topic { get; set; }

        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

}
