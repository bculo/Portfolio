﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.Application.Interfaces
{
    public interface ITransaction : IDisposable
    {
        Task StartTransaction();
        Task AbortTransaction();
        Task CommitTransaction();
    }
}
