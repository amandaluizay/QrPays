namespace QrPay.Shared.Interfaces
{
    public interface IResponseResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public interface IResponseResult<T> : IResponseResult
    {
        public T Data { get; set; }
    }
}
