using GRVAS.Training.CeuEmailTrigger.WebScrapers;

namespace GRVAS.Training.CeuEmailTrigger.Email;

internal class EmailContentGenerator : IEmailContentGenerator
{
    private readonly string _mountainsideUrl;
    private readonly string _rwjbhUrl;
    private readonly string _BergenCountyUrl;
    private readonly IMountainsideWebScraper _mountainsideWebScraper;
    private readonly IRwjbhWebScraper _rwjbhWebScraper;

    private readonly string FILE_NAME = "EmailTemplate.txt";

    public EmailContentGenerator(
        string mountainsideUrl,
        string rwjbhUrl,
        string bergenCountyUrl,
        IMountainsideWebScraper mountainsideWebScraper,
        IRwjbhWebScraper rwjbhWebScraper)
    {
        _mountainsideUrl = mountainsideUrl;
        _rwjbhUrl = rwjbhUrl;
        _BergenCountyUrl = bergenCountyUrl;
        _mountainsideWebScraper = mountainsideWebScraper;
        _rwjbhWebScraper = rwjbhWebScraper;
    }

    public string Generate(string month)
    {
        try
        {
            Console.WriteLine($"Generating email for month of {month}");

            var body = File.ReadAllText($"../../../Model/{FILE_NAME}");

            var mountainsideClasses = _mountainsideWebScraper.GetClasses(_mountainsideUrl, month);

            var rwjbhClasses = _rwjbhWebScraper.GetClasses(_rwjbhUrl, month);

            body = body.Replace("<month>", month).Replace("<RwjbhUrl>", _rwjbhUrl).Replace("<RwjbhClasses>", _stringifyClasses(rwjbhClasses))
                .Replace("<MountainsideUrl>", _mountainsideUrl).Replace("<MountainsideClasses>", _stringifyClasses(mountainsideClasses))
                .Replace("<BergenCountyURL>", _BergenCountyUrl);

            return body;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error was thrown while generating email. Exc: {e}");
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
