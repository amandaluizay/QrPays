using QrPay.Domain.Entities.Shared;

namespace QrPay.Domain.Entities
{
    public class User : Entity
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public List<Account>? Accounts { get; set; }
    }
}