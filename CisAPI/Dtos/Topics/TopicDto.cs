using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.Ideas;

namespace CisAPI.Dtos.Topics;
public class TopicDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Username { get; set; } 
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<IdeaDto> Ideas { get; set; } = new();
}
