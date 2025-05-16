using QrPay.Domain.Entities;

namespace QrPay.Shared.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(User user);
    }
}