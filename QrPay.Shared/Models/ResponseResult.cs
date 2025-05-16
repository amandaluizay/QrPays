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

    public class ResponseFileResult : ResponseResult
    {
        public byte[]? File { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        public static ResponseFileResult Sucess(byte[]? file, string? contentType = null)
        {
            return new ResponseFileResult { ContentType = contentType, File = file, IsSuccess = true, Message = "Everything work as expected" };
        }
    }

}
