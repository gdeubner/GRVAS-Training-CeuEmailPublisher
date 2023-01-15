namespace GRVAS.Training.CeuEmailTrigger.Email;

internal class EmailSender : IEmailSender
{
    private readonly IAmazonSimpleEmailService _client;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(
        IAmazonSimpleEmailService client,
        ILogger<EmailSender> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<bool> SendAsync(MimeMessage mimeMessage, string month)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await mimeMessage.WriteToAsync(memoryStream);
            var rawRequest = new SendRawEmailRequest { RawMessage = new RawMessage(memoryStream) };
            _logger.LogInformation($"Sending email for month: {month}");
            var response = await _client.SendRawEmailAsync(rawRequest);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"The email was sent successfully. month: {month}");
                mimeMessage.Dispose();
                return true;
            }
            else
            {
                _logger.LogInformation($"Failed to send email with message Id: {response.MessageId}, to: {mimeMessage.To}, due to: " +
                    $"{response.HttpStatusCode}, month: {month}.");
                mimeMessage.Dispose();
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"The email was not sent. month: {month}, Error Message [{ex.Message}]");
            mimeMessage.Dispose();
            return false;
        }
    }
}
