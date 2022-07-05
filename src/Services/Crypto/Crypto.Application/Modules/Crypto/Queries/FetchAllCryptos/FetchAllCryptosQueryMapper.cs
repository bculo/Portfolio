using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Modules.Crypto.Queries.FetchAllCryptos
{
    public class FetchAllCryptosQueryMapper : Profile
    {
        public FetchAllCryptosQueryMapper()
        {
            CreateMap<Core.Entities.Crypto, FetchAllCryptosDto>();
        }
    }
}
