using FluentValidation;
using Time.Abstract.Contracts;

namespace User.Application.Interfaces
{
    public interface IRegisterUserService
    {
        Task RegisterUser(CreateUserDto userDto);
    }

    public class CreateUserDto
    {
        public DateTime Born { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto> 
    {
        private readonly IDateTimeProvider _timeProvider;

        public CreateUserDtoValidator(IDateTimeProvider timeProvider)
        {
            _timeProvider = timeProvider;

            RuleFor(i => i.UserName)
                .MustAsync(IsUnique)
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
                .WithMessage("Person must be atleast 18 years old to use this application");
        }

        /// <summary>
        /// Is given username unqiue?
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task<bool> IsUnique(string userName, CancellationToken token)
        {
            return Task.FromResult(true);
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
