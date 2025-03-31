using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Ideas;
public class CreateIdeaDto
{
    [Required]
    [MaxLength(255)]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public Guid TopicId { get; set; }
}
