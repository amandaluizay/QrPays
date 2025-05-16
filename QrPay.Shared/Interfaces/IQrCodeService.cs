namespace QrPay.Shared.Interfaces
{
    public interface IQrCodeService
    {
        byte[] GenerateImage(string content);
        string? ReadImage(Stream content);
    }
}
