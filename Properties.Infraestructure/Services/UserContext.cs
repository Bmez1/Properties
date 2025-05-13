using Microsoft.AspNetCore.Http;

using Properties.Application.Interfaces;
using Properties.Infraestructure.Extensions;

namespace Properties.Infraestructure.Services;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        _httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");

    public string Email => 
        _httpContextAccessor
        .HttpContext?
        .User
        .GetEmail() ??
       throw new ApplicationException("User context is unavailable");
}

