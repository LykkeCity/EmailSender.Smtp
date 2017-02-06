using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Lykke.EmailSender.Smtp
{

    public class EmailSenderToSmtp : IEmailSender
    {
        private readonly EmailSenderToSmtpSettings _settings;

        public EmailSenderToSmtp(EmailSenderToSmtpSettings settings)
        {
            _settings = settings;
        }


        private MimeMessage CreateMimeMessage(EmailModel model)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            emailMessage.To.Add(new MailboxAddress(string.Empty, model.Email));
            emailMessage.Subject = model.Subject;

            return emailMessage;
        }

        public async Task SendEmailAsync(EmailModel model)
        {

            var emailMessage = CreateMimeMessage(model);


            var messageBody = new TextPart(model.IsHtml ? TextFormat.Html : TextFormat.Plain) { Text = model.Body };

            if (model.Attachments != null)
                if (model.Attachments.Length > 0)
                {
                    var multipart = new Multipart("mixed") {messageBody};

                    foreach (var attachment in model.Attachments)
                    {


                        var attachmentData = new MemoryStream(attachment.Data);
                        var attach = new MimePart(attachment.Mime)
                        {
                            ContentObject = new ContentObject(attachmentData),
                            FileName = attachment.FileName
                        };

                        multipart.Add(attach);
                    }
                    emailMessage.Body = multipart;
                }

                else
                {
                    emailMessage.Body = messageBody;
                }


            await SendEmailMessage(emailMessage);

        }

        private async Task SendEmailMessage(MimeMessage emailMessage)
        {
            using (var client = new SmtpClient())
            {
                client.LocalDomain = _settings.LocalDomain;
                await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.None);
                await client.AuthenticateAsync(_settings.Login, _settings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
