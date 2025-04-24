using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Votes;
public class CreateVoteDto
{

        [Required]
        public string? IdeaId { get; set; }

        [Required]
        [Range(-1, 1)] 
        public int Value { get; set; }
}
