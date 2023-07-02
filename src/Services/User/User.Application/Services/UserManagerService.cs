using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Common.Exceptions;
using User.Application.Entities;
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

        public async Task<UserDetailsDto> GetUserDetails(Guid userId, CancellationToken cancellationToken = default)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.ExternalId.HasValue && x.ExternalId.Value == userId);
            if (dbUser is null)
            {
                throw new PortfolioUserNotFoundException("User with given ID doesn't exists");
            }

            return MapToDto(dbUser);
        }

        private UserDetailsDto MapToDto(PortfolioUser user)
        {
            return new UserDetailsDto
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }
    }
}
