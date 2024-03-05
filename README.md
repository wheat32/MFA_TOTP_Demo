# MFATest
This is a simple console application that demonstrates the use of Multi-Factor Authentication (MFA) using Time-based One-Time Passwords (TOTP) and QR codes.
## Dependencies
The project uses the following NuGet packages:
-	Otp.NET: A .NET library for generating and validating One-Time Passwords (OTP).
-	QRCoder: A pure C# open source QR Code implementation.
-	SixLabors.ImageSharp: A cross-platform library for the processing of image files.
## How it works
The application performs the following steps:
1.	Generates a random secret key.
2.	Encodes the secret key into a QR code.
3.	Saves the QR code as an image file and opens it with the default image viewer.
4.	Starts a new task that generates a TOTP every 30 seconds using the secret key.
5.	Waits for user input and validates the entered TOTP against the generated one.
## Running the application
To run the application, you need to have .NET 8.0 SDK installed on your machine. Then, you can use the dotnet run command in the terminal from the project's directory.
## Code structure
The main logic of the application is located in the Program.cs file. The Main method is the entry point of the application.
## Note
This is a simple demonstration of MFA and should not be used as is in a production environment. Always ensure to follow best security practices when implementing MFA.
