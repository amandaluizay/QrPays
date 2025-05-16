using Microsoft.AspNetCore.Http;
using QrPay.Shared.Interfaces;
using System.Security.Claims;

namespace QrPay.Shared.Services
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string? User => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public Guid? UserId => string.IsNullOrEmpty(User)? Guid.Empty: Guid.Parse(User);

    }
}
