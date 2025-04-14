using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Votes;
public class VoteDto
{
        public int Value { get; set; }
        public DateTime VotedAt { get; set; }
        public string? IdeaTitle { get; set; }  
}
