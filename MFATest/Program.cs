using OtpNet;
using QRCoder;
using SixLabors.ImageSharp;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MFATest
{
    public class Program
    {
        public static void Main()
        {
            // Generate a random secret key
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            String base32SecretKey = Base32Encoding.ToString(secretKey);
            Console.WriteLine($"Secret Key: {base32SecretKey}");

            // Generate a QR code for the secret key
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode($"otpauth://totp/TestApp?secret={base32SecretKey}", QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            // Save the QR code image to a file
            String filePath = "qrcode.png";
            using (Image image = Image.Load(qrCodeImage))
            {
                image.Save(filePath);
            }

            // Open the image file with the default image viewer
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", filePath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", filePath);
            }

            // Start a new task to generate a TOTP every 30 seconds
            Task.Run(() =>
            {
                while (true)
                {
                    Totp totp = new Totp(secretKey);
                    String totpCode = totp.ComputeTotp();
                    Console.WriteLine($"Generated TOTP: {totpCode}");

                    // Wait for 30 seconds
                    Thread.Sleep(30000);
                }
            });

            // Main thread waits for user input and validates TOTP
            while (true)
            {
                Console.Write("Enter the TOTP: ");
                String userInput = Console.ReadLine()!;

                Totp validator = new Totp(secretKey);
                if (validator.VerifyTotp(userInput, out long timeWindowUsed))
                {
                    Console.WriteLine("TOTP is valid.");
                }
                else
                {
                    Console.WriteLine("TOTP is invalid.");
                }
            }
        }
    }
}
