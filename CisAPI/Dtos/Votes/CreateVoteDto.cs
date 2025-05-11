using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Votes;
public class CreateVoteDto
{

        public string? IdeaId { get; set; }

        public int Value { get; set; }
}