﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces.Models.Dtos
{
    public record SearchWordResDto
    {
        public string Id { get; init; }
        public DateTime Created { get; init; }
        public string SearchWord { get; init; }
        public string SearchEngineName { get; init; }
        public int SearchEngineId { get; init; }
        public string ContextTypeName { get; init; }
    }
}