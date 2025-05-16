namespace QrPay.Application.Responses
{
    public class TransactionResponse
    {
        public string? SenderName { get; set; }
        public string? SenderDocument { get; set; }
        public string? SenderAccount { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverDocument { get; set; }
        public string? ReceiverAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}
