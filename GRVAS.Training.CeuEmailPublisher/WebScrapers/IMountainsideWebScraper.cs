using GRVAS.Training.CeuEmailCreator.Model;

namespace GRVAS.Training.CeuEmailCreator.WebScrapers
{
    internal interface IMountainsideWebScraper
    {
        List<CeuClass> GetClasses(string MountainsideUrl, string month);
    }
}