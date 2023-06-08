using Stock.Application.Interfaces;

namespace Stock.API.Services
{
    public class CurrentUserService : IStockUser
    {
        public Guid Identifier => new Guid("78a46475-0e26-4ab2-b6b8-1e778ae1443c");
    }
}
