﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Core.Exceptions
{
    public class StockCoreValidationException : Exception
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public StockCoreValidationException(Dictionary<string, string[]> errors) 
            : base("Validation exception")
        {
            Errors = errors;
        }
    }
}
