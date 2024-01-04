﻿using Auth0.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Domain.Exceptions;

namespace Trend.gRPC.Services
{
    public class UserService : ICurrentUser
    {
        private readonly IAuth0AccessTokenReader _user;

        public UserService(IAuth0AccessTokenReader user)
        {
            _user = user;
        }

        public Guid UserId
        {
            get
            {
                var userId = _user.GetIdentifier();

                if (userId == Guid.Empty)
                {
                    throw new TrendAppAuthenticationException("Problem with authentication. User identifier is null");
                }

                return userId;
            }
        }
    }
}
