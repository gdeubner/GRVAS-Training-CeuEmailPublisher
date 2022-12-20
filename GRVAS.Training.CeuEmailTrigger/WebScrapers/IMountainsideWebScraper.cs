namespace GRVAS.Training.CeuEmailTrigger.WebScrapers;

internal interface IMountainsideWebScraper
{
    List<CeuClass> GetClasses(string MountainsideUrl, string month);
}