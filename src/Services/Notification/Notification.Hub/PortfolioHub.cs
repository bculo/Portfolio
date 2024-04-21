﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notification.Hub.Interfaces;


namespace Notification.Hub
{
    [Authorize]
    public class PortfolioHub : Hub<ISignalRClient>
    {
        private readonly ILogger<PortfolioHub> _logger;

        public PortfolioHub(ILogger<PortfolioHub> logger)
        {
            _logger = logger;
        }

        private string FormatGroupName(string groupName, string assetType)
        {
            ArgumentException.ThrowIfNullOrEmpty(groupName);
            ArgumentException.ThrowIfNullOrEmpty(assetType);

            return $"{groupName}-{assetType}";
        }
        
        public override Task OnConnectedAsync()
        {
            _logger.LogTrace("Connection id {ConnectionId} connected", Context.ConnectionId);
            
            return base.OnConnectedAsync();
        }
        
        public async Task JoinToGroup(string groupName, string assetType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, FormatGroupName(groupName, assetType));
        }

        public async Task LeaveGroup(string groupName, string assetType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, FormatGroupName(groupName, assetType));
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogTrace("Connection id {ConnectionId} disconnected", Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
