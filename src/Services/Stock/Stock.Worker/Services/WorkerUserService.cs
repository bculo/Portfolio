using Stock.Application.Interfaces;

namespace Stock.Worker.Services
{
    public class WorkerUserService : IStockUser
    {
        public static Guid WorkerIdentifer = new Guid("07de1a1d-ccbb-4eb2-bb47-a64895b58e4d");
        public Guid Identifier => WorkerIdentifer;
    }
}
