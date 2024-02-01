using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Common;

public record PageRequestDto
{
    public int Page { get; set; } = default!;
    public int Take { get; set; } = default!;
}

