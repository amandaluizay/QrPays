using QrPay.Domain.Interfaces;

namespace QrPay.Domain.Entities.Shared
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
