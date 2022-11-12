using GRVAS.Training.CeuEmailCreator.Model;

namespace GRVAS.Training.CeuEmailCreator.WebScrapers
{
    internal interface IRwjbhWebScraper
    {
        List<CeuClass> GetClasses(string RwjUrl, string month);
    }
}