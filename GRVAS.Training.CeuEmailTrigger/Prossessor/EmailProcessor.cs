using GRVAS.Training.CeuEmailTrigger.Validation;

namespace GRVAS.Training.CeuEmailTrigger.Prossessor;

internal class EmailProcessor : IEmailProcessor
{
    private readonly string _mountainsideUrl;
    private readonly string _rwjbhUrl;
    private readonly IEmailContentGenerator _emailContentGenerator;
    private readonly IEmailGenerator _emailGenerator;
    private readonly IEmailSender _emailSender;
    private readonly IMountainsideWebScraper _mountainsideWebScraper;
    private readonly IRwjbhWebScraper _rwjbhWebScraper;
    private readonly IClassValidator _classValidator;
    private readonly ILogger<EmailProcessor> _logger;

    public EmailProcessor(
        string mountainsideUrl,
        string rwjbhUrl,
        IEmailContentGenerator emailContentGenerator,
        IEmailGenerator emailGenerator,
        IEmailSender emailSender,
        IMountainsideWebScraper mountainsideWebScraper,
        IRwjbhWebScraper rwjbhWebScraper,
        IClassValidator classValidator,
        ILogger<EmailProcessor> logger)
    {
        _mountainsideUrl = mountainsideUrl;
        _rwjbhUrl = rwjbhUrl;
        _emailContentGenerator = emailContentGenerator;
        _emailGenerator = emailGenerator;
        _emailSender = emailSender;
        _mountainsideWebScraper = mountainsideWebScraper;
        _rwjbhWebScraper = rwjbhWebScraper;
        _classValidator = classValidator;
        _logger = logger;
    }

    public async Task<bool> ProcessAsync()
    {
        try
        {
            _logger.LogInformation($"Starting Email Processor");

            //call email generator
            var month = DateTime.Now.ToString("MMMM");

            var mountainsideClasses = _mountainsideWebScraper.GetClasses(_mountainsideUrl, month);
            var rwjbhClasses = _rwjbhWebScraper.GetClasses(_rwjbhUrl, month);

            var emailContent = _emailContentGenerator.Generate(month, mountainsideClasses, rwjbhClasses);
            _logger.LogInformation($"Email generated: {emailContent}");

            var mimeEmail = _emailGenerator.Generate(emailContent, month, 
                _classValidator.Validate(new List<List<CeuClass>> { mountainsideClasses, rwjbhClasses }));

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
