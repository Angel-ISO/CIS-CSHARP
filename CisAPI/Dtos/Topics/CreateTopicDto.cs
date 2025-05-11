using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Dtos.Topics
{
    public class CreateTopicDto
    {
    public string? Title { get; set; }
    public string? Description { get; set; }
    }
}