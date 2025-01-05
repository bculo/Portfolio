using AutoMapper;

namespace Crypto.Application.Modules.Crypto.Commands.AddNew
{
    public class AddNewCommandMapper : Profile
    {
        public AddNewCommandMapper()
        {
            CreateMap<AddNewCommand, Core.Entities.CryptoEntity>()
                .ForMember(dst => dst.Symbol, opt => opt.MapFrom(src => src.Symbol.ToUpper()))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Symbol.ToUpper()));
        }   
    }
}
