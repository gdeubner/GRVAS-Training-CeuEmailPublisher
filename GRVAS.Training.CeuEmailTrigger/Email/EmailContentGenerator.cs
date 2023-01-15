namespace GRVAS.Training.CeuEmailTrigger.Email;

internal class EmailContentGenerator : IEmailContentGenerator
{
    private readonly string _mountainsideUrl;
    private readonly string _rwjbhUrl;
    private readonly string _BergenCountyUrl;
    private readonly IMountainsideWebScraper _mountainsideWebScraper;
    private readonly IRwjbhWebScraper _rwjbhWebScraper;
    private readonly ILogger<EmailContentGenerator> _logger;

    private readonly string FILE_NAME = "EmailTemplate.txt";

    public EmailContentGenerator(
        string mountainsideUrl,
        string rwjbhUrl,
        string bergenCountyUrl,
        IMountainsideWebScraper mountainsideWebScraper,
        IRwjbhWebScraper rwjbhWebScraper,
        ILogger<EmailContentGenerator> logger)
    {
        _mountainsideUrl = mountainsideUrl;
        _rwjbhUrl = rwjbhUrl;
        _BergenCountyUrl = bergenCountyUrl;
        _mountainsideWebScraper = mountainsideWebScraper;
        _rwjbhWebScraper = rwjbhWebScraper;
        _logger = logger;
    }

    public string Generate(string month)
    {
        try
        {
            _logger.LogInformation($"Generating email for month of {month}");
            
            var body = File.ReadAllText($"{Directory.GetCurrentDirectory()}/Model/{FILE_NAME}");

            var mountainsideClasses = _mountainsideWebScraper.GetClasses(_mountainsideUrl, month);

            var rwjbhClasses = _rwjbhWebScraper.GetClasses(_rwjbhUrl, month);

            body = body.Replace("<month>", month)
                .Replace("<RwjbhUrl>", _rwjbhUrl)
                .Replace("<RwjbhClasses>", rwjbhClasses.Count > 0 ? _stringifyClasses(rwjbhClasses)
                    : $"There are no RWJBH classes for the month of {month}.")
                .Replace("<MountainsideUrl>", _mountainsideUrl)
                .Replace("<MountainsideClasses>", mountainsideClasses.Count > 0 ? _stringifyClasses(mountainsideClasses) 
                    : $"There are no Mountainside classes for the month of {month}.")
                .Replace("<BergenCountyURL>", _BergenCountyUrl);

            return body;
        }
        catch (Exception e)
        {
            _logger.LogError($"An error was thrown while generating email. Current working directory: {Directory.GetCurrentDirectory()} Exc: {e}");
            return null;
        }
    }

    private string _stringifyClasses(List<CeuClass> classes)
    {
        var builder = new StringBuilder();

        foreach (var c in classes)
        {
            builder.Append($"\n\n{c.FormClassOutput()}");
        }

        return builder.ToString();
    }
}
