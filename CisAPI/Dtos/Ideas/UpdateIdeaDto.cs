using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Ideas
{
    public class UpdateIdeaDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}