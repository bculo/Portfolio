using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Interfaces.Services
{
    public interface IHubConnectionTracker
    {
        Task AddUserConnection(string userIdentifier, string connectionIdentifier);
        Task RemoveSingleUserConnection(string connectionIdentifier);
        Task RemoveUserConnection(string connectionIdentifier);
        Task AddToGroup(string groupIdentifier, string connectionIdentifier);
        Task RemoveFromGroup(string groupIdentifier, string connectionIdentifier);
    }
}
