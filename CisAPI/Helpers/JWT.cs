using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisAPI.Helpers;

public class JWT
{

    public string? Issuer { get; set; }
    public string? Key { get; set; }
    public int? DurationInMinutes { get; set; }

}
