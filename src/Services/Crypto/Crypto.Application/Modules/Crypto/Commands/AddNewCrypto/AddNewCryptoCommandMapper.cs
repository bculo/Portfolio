using AutoMapper;

namespace Crypto.Application.Modules.Crypto.Commands.AddNewCrpyto
{
    public class AddNewCryptoCommandMapper : Profile
    {
        public AddNewCryptoCommandMapper()
        {
            CreateMap<AddNewCryptoCommand, Core.Entities.Crypto>()
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol.ToUpper()))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Symbol.ToUpper()));
        }   
    }
}
