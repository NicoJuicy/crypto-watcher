﻿
using CryptoWatcher.Api.Responses;
using MediatR;

namespace CryptoWatcher.Api.Requests
{
    public class AddUserRequest: IRequest<UserResponse>
    {
    public string UserId { get; set; }
    }
}
