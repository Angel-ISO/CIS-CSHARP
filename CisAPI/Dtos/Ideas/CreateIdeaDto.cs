using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Ideas
{
    public class CreateIdeaDto
    {
        public string? TopicId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
    }
}