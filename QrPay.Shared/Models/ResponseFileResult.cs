namespace QrPay.Shared.Models
{
    public class ResponseFileResult : ResponseResult
    {
        public byte[]? File { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }

        public static ResponseFileResult Sucess(byte[] file, string fileName)
        {
            return new ResponseFileResult
            {
                ContentType = GetContentType(fileName),
                File = file,
                FileName = fileName,
                IsSuccess = true,
                Message = "Everything work as expected"
            };
        }

        public static ResponseFileResult Fail(string message)
        {
            return new ResponseFileResult
            {
                IsSuccess = false,
                Message = message
            };
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".zip" => "application/zip",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".json" => "application/json",
                ".xml" => "application/xml",
                ".html" => "text/html",
                _ => "application/octet-stream" 
            };
        }

    }

}
