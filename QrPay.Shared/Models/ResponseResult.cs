using QrPay.Shared.Interfaces;

namespace QrPay.Shared.Models
{
    public class ResponseResult : IResponseResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ResponseResult Sucess()
        {
            return new ResponseResult
            {
                IsSuccess = true,
                Message = "Everything work as expected"
            };
        }

        public static ResponseResult Fail(string message)
        {
            return new ResponseResult
            {
                IsSuccess = false,
                Message = message
            };
        }
    }

    public class ResponseResult<T> : ResponseResult, IResponseResult<T>
        where T : class
    {
        public T? Data { get; set; }

        public static ResponseResult<T> Sucess(T? data)
        {
            return new ResponseResult<T>
            {
                IsSuccess = true,
                Message = "Everything work as expected",
                Data = data
            };
        }
    }

}
