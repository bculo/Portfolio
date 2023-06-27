using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interfaces;
using User.Application.Persistence;

namespace User.Application.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserDbContext _context;

        public UserManagerService(UserDbContext context)
        {
            _context = context;
        }

        public Task<UserDetailsDto> GetUserDetails(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
