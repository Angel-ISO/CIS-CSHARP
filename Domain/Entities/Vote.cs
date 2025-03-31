using System.ComponentModel.DataAnnotations;


namespace Domain.Entities;
public class Vote : BaseEntity
{
         [Required]
        public Guid UserId { get; set; }


        [Required]
        [Range(-1, 1)]
        public int Value { get; set; } 

        public DateTime VotedAt { get; set; } = DateTime.UtcNow;


        [Required]
        public Guid IdeaId { get; set; }

        public virtual Idea? Idea { get; set; }
}
