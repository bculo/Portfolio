using FluentValidation;
using Microsoft.Extensions.Logging;
using User.Application.Interfaces;
using User.Application.Persistence;

namespace User.Application.Services
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly UserDbContext _context;
        private readonly IValidator<CreateUserDto> _validator;
        private readonly ILogger<RegisterUserService> _logger;

        public RegisterUserService(ILogger<RegisterUserService> logger,
            IValidator<CreateUserDto> validator,
            UserDbContext context)
        {
            _logger = logger;
            _validator = validator;
            _context = context;
        }

        public async Task RegisterUser(CreateUserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
