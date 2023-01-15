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

            var tableItemXPath = "//div[contains(@class,\"col-md-6 col-lg-6\")]";

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
        return list.Where(x => x.Date.Contains(month) && !x.IsInPerson).ToList();
    }

    private List<CeuClass> _getMountainsideClasses(HtmlNodeCollection classCollection)
    {
        try
        {
            List<CeuClass> classList = new List<CeuClass>();
            List<HtmlNode> HtmlColumnNodeList = classCollection.ToList();
            List<HtmlNode> HtmlNodeList = HtmlColumnNodeList[0].ChildNodes.ToList();
            HtmlNodeList.AddRange(HtmlColumnNodeList[1].ChildNodes.ToList());
            CeuClass mClass = null;
            foreach (var node in HtmlNodeList)
            {
                if (node.Name.Equals("h3"))
                {
                    if (mClass != null)
                    {
                        classList.Add(mClass);
                    }
                    mClass = new CeuClass();
                    mClass.Cost = "0.00";
                    mClass.Time = "6:30pm";
                    mClass.Description = "6:30pm - Dinner and Drinks, 7pm - Lecture";
                    mClass.Date = node.GetDirectInnerText().Trim();
                }
                else if (mClass != null && node.GetClasses().Contains("bold"))
                {
                    mClass.Title = node.GetDirectInnerText().Trim();
                }
                else if (mClass != null && node.GetClasses().Contains("no-margin"))
                {
                    mClass.LocationName = node.GetDirectInnerText().Trim();
                }
            }

            return classList;
        }
        catch (Exception e)
        {
            _logger.LogError($"An error occured while parsing a class: Exc: {e}");
            return new List<CeuClass>();
        }
    }
}
