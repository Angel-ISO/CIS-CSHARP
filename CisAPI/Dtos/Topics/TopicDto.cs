using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CisAPI.Dtos.Ideas;

namespace CisAPI.Dtos.Topics
{
    public class TopicDto
    {
    public string? Id { get; set; }
    public string? Title { get; set; } 
    public string? Description { get; set; } 
    public string? Username { get; set; } 
    public DateTime CreatedAt { get; set; }
    public List<IdeaDto> Ideas { get; set; } = new();
    
    }
}