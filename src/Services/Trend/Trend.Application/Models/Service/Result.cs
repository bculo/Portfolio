using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Models.Service
{
    public class Result
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }

        public Result()
        {
            Success = true;
        }

        public Result(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }
    }
}
