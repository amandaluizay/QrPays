using Microsoft.AspNetCore.Http;
using QrPay.Shared.Interfaces;
using System.Security.Claims;

namespace QrPay.Shared.Services
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private string? User => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public Guid UserId => string.IsNullOrEmpty(User)? Guid.Empty: Guid.Parse(User);
        
        private string? Name => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        public string UserName => string.IsNullOrEmpty(Name) ? "system without user" : Name;

    }
}
