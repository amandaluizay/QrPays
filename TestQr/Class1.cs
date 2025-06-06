﻿using QRCoder;
using System.Drawing;

namespace TestQr
{
    public static class QrCodeGenerator
    {
        public static string Teste { get; set; } = "teste";
        public static byte[] GenerateImage(string url)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(10);
            return qrCodeImage;
        }

        //public static byte[] GenerateByteArray(string url)
        //{
        //    var image = GenerateImage(url);
        //    return ImageToByte(image);
        //}

        //private static byte[] ImageToByte(Image img)
        //{
        //    using var stream = new MemoryStream();
        //    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //    return stream.ToArray();
        //}
    }
}
