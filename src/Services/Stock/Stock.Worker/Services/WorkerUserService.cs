using Stock.Application.Interfaces.User;

namespace Stock.Worker.Services
{
    public class WorkerUserService : IStockUser
    {
        private static readonly Guid WorkerIdentifier = new Guid("07de1a1d-ccbb-4eb2-bb47-a64895b58e4d");
        public Guid Identifier => WorkerIdentifier;
    }
}
