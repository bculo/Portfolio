using Events.Common.User;
using MassTransit;
using User.Application.Interfaces;

namespace User.Application.Consumers
{
    public class UserNotSavedToPersistenceStorageConsumer : IConsumer<UserNotSavedToPersistenceStorage>
    {
        private readonly IRegisterUserService _service;

        public UserNotSavedToPersistenceStorageConsumer(IRegisterUserService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<UserNotSavedToPersistenceStorage> context)
        {
            var message = context.Message;

            var entity = new UserBaseInfoDto
            {
                FirstName = message.FirstName,
                LastName = message.LastName,
                Born = message.BornOn,
                UserName = message.UserName,  
            };

            await _service.AddUserToStorage(entity, CancellationToken.None);
        }
    }
}
