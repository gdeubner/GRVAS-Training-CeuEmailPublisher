namespace GRVAS.Training.CeuEmailTrigger.Prossessor;

internal class EmailProcessor : IEmailProcessor
{
    private readonly IEmailContentGenerator _emailContentGenerator;
    private readonly IEmailGenerator _emailGenerator;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailProcessor> _logger;

    public EmailProcessor(
        IEmailContentGenerator emailContentGenerator,
        IEmailGenerator emailGenerator,
        IEmailSender emailSender,
        ILogger<EmailProcessor> logger)
    {
        _emailContentGenerator = emailContentGenerator;
        _emailGenerator = emailGenerator;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task<bool> ProcessAsync()
    {
        try
        {
            _logger.LogInformation($"Starting Email Processor");

            //call email generator
            var month = DateTime.Now.ToString("MMMM");

            var emailContent = _emailContentGenerator.Generate(month);
            _logger.LogInformation($"Email generated: {emailContent}");

            var mimeEmail = _emailGenerator.Generate(emailContent, month);

            var success = await _emailSender.SendAsync(mimeEmail, month);

            _logger.LogInformation(success ? $"Successfully sent email" : "Failed to send email");


            return true;
        }
        catch (Exception e)
        {
            _logger.LogError($"Processing failed. Exc: {e}");
            return false;
        }

    }
}
