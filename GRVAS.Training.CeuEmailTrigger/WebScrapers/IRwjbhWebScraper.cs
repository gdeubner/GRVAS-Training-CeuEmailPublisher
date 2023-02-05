namespace GRVAS.Training.CeuEmailTrigger.WebScrapers;

internal interface IRwjbhWebScraper
{
    List<CeuClass>? GetClasses(string RwjUrl, string month);
}