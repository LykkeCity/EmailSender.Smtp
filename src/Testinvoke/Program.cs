using System;
using Common;
using Lykke.EmailSender;
using Lykke.EmailSender.Smtp;

namespace Testinvoke
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var settings = new EmailSenderToSmtpSettings
            {
                Host = "",
                Port = 25,
                Login = "",
                Password = "",
                DisplayName = "",
                From = "",
                LocalDomain = ""

            };

            var sender = new EmailSenderToSmtp(settings);

            var email = new EmailModel
            {
                Email = "",
                Subject = "My subject",
                Body = "My body",
                IsHtml = true,
                Attachments = new []
                {
                    new EmailAttachment
                    {
                        FileName = "Text.txt",
                        Mime = "text/plain",
                        Data = "This is file content".ToUtf8Bytes()
                    }
                }
            };

            sender.SendEmailAsync(email).Wait();

            Console.WriteLine("sent");

            Console.ReadLine();
        }
    }
}
