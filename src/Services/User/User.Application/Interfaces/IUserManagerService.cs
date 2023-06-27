using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Interfaces
{
    public interface IUserManagerService
    {
        Task<UserDetailsDto> GetUserDetails(Guid userId);
    }

    public class UserDetailsDto
    {

    }
}
