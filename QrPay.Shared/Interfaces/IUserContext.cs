namespace QrPay.Shared.Interfaces
{
    public interface IUserContext
    {
        Guid UserId { get; }
        string UserName { get; }
    }
}