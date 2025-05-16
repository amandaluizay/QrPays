using QrPay.Domain.Entities.Shared;

namespace QrPay.Domain.Entities
{
    public class Account : Entity
    {
        public string? AccountNumber { get; set; }
        public string? Document { get; set; }
        public string? Name { get; set; }
        public decimal? Balance { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
