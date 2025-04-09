using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Votes;
public class UpdateVoteDto
{
        public Guid IdeaId { get; set; }

        [Range(-1, 1)] 
        public int Value { get; set; }
}
