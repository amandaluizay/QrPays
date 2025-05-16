using System.Text.Json.Serialization;

namespace QrPay.Application.Requests
{
    public record class TransactionRequest
    {
        public string? ReceiverName { get; set; }
        public string? ReceiverDocument { get; set; }
        public string? ReceiverAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}

