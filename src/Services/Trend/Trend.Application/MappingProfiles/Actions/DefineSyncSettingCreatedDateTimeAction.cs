﻿using AutoMapper;
using Dtos.Common.v1.Trend.SearchWord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles.Actions
{
    public class DefineSyncSettingCreatedDateTimeAction : IMappingAction<SearchWordCreateDto, Domain.Entities.SearchWord>
    {
        private readonly IDateTimeProvider _time;

        public DefineSyncSettingCreatedDateTimeAction(IDateTimeProvider time)
        {
            _time = time;
        }

        public void Process(SearchWordCreateDto source, Domain.Entities.SearchWord destination, ResolutionContext context)
        {
            destination.Created = _time.Now;
            destination.IsActive = true;
        }
    }
}
