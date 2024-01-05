using AutoMapper;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Entities;

namespace Trend.Application.MappingProfiles.Actions
{
    public class AuditableDocumentTimeAction<T> 
        : IMappingAction<T, AuditableDocument> where T : notnull
    {
        private readonly IDateTimeProvider _time;

        public AuditableDocumentTimeAction(IDateTimeProvider time)
        {
            _time = time;
        }
        
        public void Process(T source, AuditableDocument destination, ResolutionContext context)
        {
            destination.Created = _time.Now;
            destination.IsActive = true;
        }
    }
}
