using System.Data.Common;

namespace GRVAS.Training.CeuEmailCreator.WebScrapers;

internal class MountainsideWebScraper : IMountainsideWebScraper
{
    private readonly ILogger<MountainsideWebScraper> _logger;

    public MountainsideWebScraper(ILogger<MountainsideWebScraper> logger)
    {
        _logger = logger;
    }

    public List<CeuClass> GetClasses(string MountainsideUrl, string month)
    {
        try
        {
            _logger.LogInformation("Getting Mountainside classes");
            HtmlNodeCollection classNodes = _getMountainsideClassDetails(MountainsideUrl);
            return _filter(_getMountainsideClasses(classNodes), month);
        }
        catch (Exception e)
        {
            _logger.LogError(e.StackTrace);
            return new List<CeuClass>();
        }
    }

    private HtmlNodeCollection _getMountainsideClassDetails(string url)
    {
        try
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            var tableItemXPath = "//div[contains(@class,\"well mt-35\")]";

            HtmlNodeCollection classNodeCollection = doc.DocumentNode.SelectNodes(tableItemXPath);

            return classNodeCollection;
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occured while loading the html for url: {url} Exc: {e}");
            return null;
        }
    }

    private List<CeuClass> _filter(List<CeuClass> list, string month)
    {
        return list.Where(x => x.Date.Contains(month)).ToList();
    }

    private List<CeuClass> _getMountainsideClasses(HtmlNodeCollection classCollection)
    {
        try
        {
            List<HtmlNode> htmlColumnNodeList = classCollection.ToList();
            List<HtmlNode> htmlNodeList = htmlColumnNodeList[0].ChildNodes.ToList();

            var ceuClass = new CeuClass()
            {
                Date = htmlNodeList[1].ChildNodes[0].InnerHtml.Trim(),
                Note = htmlNodeList[2].ChildNodes[1].InnerHtml.Trim(),
                Time = htmlNodeList[2].ChildNodes[3].InnerHtml.Trim(),
                Ceus = _getCEUs(htmlNodeList[2].ChildNodes[5].InnerHtml),
                Title = htmlNodeList[2].ChildNodes[7].InnerHtml.Trim(),
                Description = htmlNodeList[4].InnerHtml.Trim(),
                State = "New Jersey",
                StreetAddress = "1 Bay Avenue",
                Town = "Montclair",
                IsInPerson= true,
                LocationName = "Mountainside Medical Center",
                Cost = 0
            };


            return new List<CeuClass> { ceuClass };
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occured while parsing a class: Exc: {e}");
            return new List<CeuClass>();
        }
    }

    private double? _getCEUs(string ceu)
    {
        if (double.TryParse(string.Concat(ceu.Where(c => "1234567890.".Contains(c))), out var ceus))
        {
            return ceus;
        }
        return null;
    }
}
