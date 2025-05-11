using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Ideas
{

    public class IdeaDto
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}