using Stock.Application.Interfaces;

namespace Stock.Worker.Services
{
    /// <summary>
    /// A service that gives to Worker application unique identifier (Used in DBContext for trackable entities) 
    /// </summary>
    public class WorkerUserService : IStockUser
    {
        public static Guid WorkerIdentifer = new Guid("07de1a1d-ccbb-4eb2-bb47-a64895b58e4d");
        public Guid Identifier => WorkerIdentifer;
    }
}
