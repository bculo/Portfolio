using AutoMapper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common.Contracts;
using Trend.Application.Models.Dtos.Google;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles.Actions
{
    public class DefineArticleCreatedDateTimeAction : IMappingAction<GoogleSearchEngineItemDto, Article>
    {
        private readonly IDateTimeProvider _time;

        public DefineArticleCreatedDateTimeAction(IDateTimeProvider time)
        {
            _time = time;
        }

        public void Process(GoogleSearchEngineItemDto source, Article destination, ResolutionContext context)
        {
            //destination.Id = ObjectId.GenerateNewId().ToString();
            destination.IsActive = true;
            destination.Created = _time.Now;
        }
    }
}
