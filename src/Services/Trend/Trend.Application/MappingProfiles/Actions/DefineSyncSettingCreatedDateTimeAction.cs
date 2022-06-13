using AutoMapper;
using Dtos.Common.v1.Trend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles.Actions
{
    public class DefineSyncSettingCreatedDateTimeAction : IMappingAction<SyncSettingCreateDto, SyncSetting>
    {
        private readonly IDateTime _time;

        public DefineSyncSettingCreatedDateTimeAction(IDateTime time)
        {
            _time = time;
        }

        public void Process(SyncSettingCreateDto source, SyncSetting destination, ResolutionContext context)
        {
            destination.Created = _time.DateTime;
            destination.IsActive = true;
        }
    }
}
