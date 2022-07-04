using AutoMapper;
using Crypto.Application.Common.Mappings;
using Crypto.Core.Interfaces;
using Time.Common.Contracts;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto
{
    public class AddNewCryptoCommandMapper : Profile
    {
        public AddNewCryptoCommandMapper()
        {
            CreateMap<AddNewCryptoCommand, Core.Entities.Crypto>()
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol))
                .ForMember(dst => dst.UpdateAutomatically, opt => opt.MapFrom(src => true))
                .AfterMap<AttachTimeAction>();
        }   
    }
}
