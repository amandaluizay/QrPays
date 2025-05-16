using QRCoder;
using QrPay.Shared.Interfaces;
using System.Drawing;
using ZXing.Windows.Compatibility;

namespace QrPay.Shared.Services
{
    public class QrCodeService : IQrCodeService
    {
        public byte[] GenerateImage(string content)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            
            return qrCode.GetGraphic(10);
        }

        public string? ReadImage(Stream content)
        {
            var reader = new BarcodeReader();

            var barcodeBitmap = (Bitmap)Image.FromStream(content);

            var result = reader.Decode(barcodeBitmap);

            if (result != null)
            {
                return result.Text;
            }

            return null;
        }
    }
}
