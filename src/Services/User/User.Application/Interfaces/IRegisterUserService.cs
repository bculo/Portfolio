using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;
using User.Application.Persistence;

namespace User.Application.Interfaces
{
    public interface IRegisterUserService
    {
        Task ApproveUser(long userId, CancellationToken token = default);
    }

    public class CreateUserDto
    {
        public DateTime Born { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
