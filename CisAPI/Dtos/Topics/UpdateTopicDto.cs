using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Topics;
public class UpdateTopicDto
{
    [MaxLength(255)]
    public string? Title { get; set; }

    public string ? Description { get; set; } 
}
