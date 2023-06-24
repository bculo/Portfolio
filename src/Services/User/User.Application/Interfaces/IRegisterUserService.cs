using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Time.Abstract.Contracts;
using User.Application.Persistence;

namespace User.Application.Interfaces
{
    public interface IRegisterUserService
    {
        Task RegisterUser(CreateUserDto userDto, CancellationToken token = default);
        Task AddUserToStorage(UserBaseInfoDto instance, CancellationToken token = default);
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


    public class UserBaseInfoDto
    {
        public DateTime Born { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto> 
    {
        private readonly UserDbContext _context;
        private readonly IDateTimeProvider _timeProvider;
        
        public CreateUserDtoValidator(IDateTimeProvider timeProvider, UserDbContext context)
        {
            _timeProvider = timeProvider;
            _context = context;

            RuleFor(i => i.UserName)
                .MustAsync(IsUnique)
                .WithMessage("Given username is already taken.")
                .MinimumLength(5)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(i => i.FirstName)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(i => i.LastName)
                .MaximumLength(128)
                .NotEmpty();

            RuleFor(i => i.Born)
                .Must(IsAdultPerson)
                .WithMessage("Person must be atleast 18 years old to use this application.")
                .NotEmpty();

            RuleFor(i => i.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(i => i.Password)
                .MinimumLength(4)
                .NotEmpty();
        }

        /// <summary>
        /// Is given username unqiue?
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> IsUnique(string userName, CancellationToken token)
        {
            return await _context.Users.AllAsync(i => i.UserName != userName, token);
        }

        /// <summary>
        /// Is user adult / is atleast 18 years old ?
        /// </summary>
        /// <param name="bornOn"></param>
        /// <returns></returns>
        private bool IsAdultPerson(DateTime bornOn)
        {
            if((_timeProvider.Now.Year - bornOn.Year) < 18)
            {
                return false;
            }

            return true;
        }
    }
}
