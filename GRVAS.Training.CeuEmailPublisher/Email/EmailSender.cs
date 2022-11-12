using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MimeKit;
using System.Net;

namespace GRVAS.Training.CeuEmailCreator.Email;

internal class EmailSender : IEmailSender
{
    private readonly IAmazonSimpleEmailService _client;


    public EmailSender(IAmazonSimpleEmailService client)
    {
        _client = client;
    }

    public async Task<bool> SendAsync(MimeMessage mimeMessage, string month)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await mimeMessage.WriteToAsync(memoryStream);
            var rawRequest = new SendRawEmailRequest { RawMessage = new RawMessage(memoryStream) };
            Console.WriteLine($"Sending email for month: {month}");
            var response = await _client.SendRawEmailAsync(rawRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"The email was sent successfully. month: {month}");
                mimeMessage.Dispose();
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to send email with message Id: {response.MessageId}, to: {mimeMessage.To}, due to: " +
                    $"{response.HttpStatusCode}, month: {month}.");
                mimeMessage.Dispose();
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The email was not sent. month: {month}, Error Message [{ex.Message}]");
            mimeMessage.Dispose();
            return false;
        }
    }
}
