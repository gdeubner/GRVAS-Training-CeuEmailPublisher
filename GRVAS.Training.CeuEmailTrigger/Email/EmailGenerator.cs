namespace GRVAS.Training.CeuEmailTrigger.Email;

internal class EmailGenerator : IEmailGenerator
{
    private readonly string _sender;
    private readonly string _recipient;
    private readonly string _failureRecipient;
    private readonly ILogger<EmailGenerator> _logger;

    public EmailGenerator(
        string sender,
        string recipient,
        string failureRecipient,
        ILogger<EmailGenerator> logger)
    {
        _sender = sender;
        _recipient = recipient;
        _failureRecipient = failureRecipient;
        _logger = logger;
    }

    public MimeMessage Generate(string emailBodyContent, string month, bool classesValid)
    {
        try
        {
            var message = new MimeMessage();

            message.To.Add(new MailboxAddress(string.Empty, classesValid? _recipient : _failureRecipient));

            message.From.Add(new MailboxAddress(string.Empty, _sender));
            message.Subject = classesValid ? $"GRVAS: {month} CEU Opportunities" : $"***GRVAS: INVALID CLASS FORMAT FOR MONTH {month}***";

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
