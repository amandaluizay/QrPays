using System.Drawing;
using ZXing.Windows.Compatibility;

var path = @"C:\Users\amanda\Downloads\qrCode.png";

//var qrGenerator = new QRCodeGenerator();
//var qrCodeData = qrGenerator.CreateQrCode("testezinho", QRCodeGenerator.ECCLevel.Q);
//var qrCode = new BitmapByteQRCode(qrCodeData);
//var qrCodeImage = qrCode.GetGraphic(10);
//File.WriteAllBytes(path, qrCodeImage);


// create a barcode reader instance
var reader = new BarcodeReader();
// load a bitmap
var barcodeBitmap = (Bitmap)Image.FromFile(path);
// detect and decode the barcode inside the bitmap
var result = reader.Decode(barcodeBitmap);
// do something with the result
if (result != null)
{
    var r = result.Text;
    ;
}
else
{
    Console.WriteLine("No barcode found");
}

