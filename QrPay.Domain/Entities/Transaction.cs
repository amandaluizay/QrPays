using QrPay.Domain.Entities.Shared;
using QrPay.Domain.Enums;

namespace QrPay.Domain.Entities
{
    public class Transaction : Entity
    {
        public Account Account { get; set; }
        public Guid AccountId { get; set; }

        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public ETransactionType? Type { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
