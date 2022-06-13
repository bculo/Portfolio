﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Domain.Interfaces
{
    public interface ISyncSettingRepository : IRepository<SyncSetting>
    {
        Task<bool> IsDuplicate(string searchWord, SearchEngine engine);
    }
}
