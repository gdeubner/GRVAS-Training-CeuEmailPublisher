namespace GRVAS.Training.CeuEmailTrigger.Email;

internal class EmailGenerator : IEmailGenerator
{
    private readonly string _sender;
    private readonly string _recipient;
    private readonly ILogger<EmailGenerator> _logger;

    public EmailGenerator(
        string sender,
        string recipient,
        ILogger<EmailGenerator> logger)
    {
        _sender = sender;
        _recipient = recipient;
        _logger = logger;
    }

    public MimeMessage Generate(string emailBodyContent, string month)
    {
        try
        {
            var message = new MimeMessage();

            message.To.Add(new MailboxAddress(string.Empty, _recipient));

            message.From.Add(new MailboxAddress(string.Empty, _sender));
            message.Subject = $"GRVAS {month} CEU Opportunities";

            var emailBody = new BodyBuilder();
            
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = emailBodyContent;

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }
        catch (Exception e)
        {
            _logger.LogError($"Failed to generate email body. Exc: [{e}]");
            return null;
        }
    }
}
