using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Topics;
public class CreateTopicDto
{
    [Required]
    [MaxLength(255)]
    public string ? Title { get; set; }

    [Required]
    public string ? Description { get; set; }
}
