﻿namespace GRVAS.Training.CeuEmailCreator.WebScrapers;

internal class RwjbhWebScraper : IRwjbhWebScraper
{
    private readonly ILogger<RwjbhWebScraper> _logger;

    public RwjbhWebScraper(ILogger<RwjbhWebScraper> logger)
    {
        _logger = logger;
    }

    public List<CeuClass>? GetClasses(string RwjUrl, string month)
    {
        try
        {
            _logger.LogInformation("Getting RWJBH classes");
            HtmlNodeCollection classNodes = _getRWJClassDetails(RwjUrl);
            return _filter(_parseClassDetails(classNodes), month);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return null;
        }
    }

    private HtmlNodeCollection? _getRWJClassDetails(string url)
    {
        try
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            var tableItemXPath = "//tr[contains(@class,\"bottomborder\")]/td/table/tr[2]";

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
        return list.Where(x => x.Date.Contains(month) && x.Cost < 10).ToList();
    }

    private List<CeuClass>? _parseClassDetails(HtmlNodeCollection classNodes)
    {
        var classes = new List<CeuClass>();
        foreach (var node in classNodes)
        {
            var newClass = new CeuClass();
            try
            {
                IEnumerable<HtmlNode> nodeCollection = node.Elements("td");
                List<HtmlNode> childList = nodeCollection.ToList();
                newClass.Title = childList[1].Element("span").GetDirectInnerText();
                newClass.Note = childList[1].Elements("span").ToList()[1].Element("i").GetDirectInnerText().Split("Note:")[1].Trim();
                newClass.Date = childList[1].Element("div").Element("span").GetDirectInnerText();
                string[] timeParse = childList[1].Element("div").Elements("span").ToList()[1].GetDirectInnerText().Split("&nbsp;");
                if (timeParse.Length > 1)
                {
                    newClass.Time = timeParse[1];
                }
                else
                {
                    newClass.Time = timeParse[0];
                }

                newClass.Description = childList[1].Element("a").Element("span").GetDirectInnerText().Trim();

                //for online classes, locationName is ONLINE LIVE CLASS
                newClass.LocationName = childList[2].Elements("span").ToList()[0].GetDirectInnerText();
                //for online classes, streetAddress is the website url
                newClass.StreetAddress = childList[2].Elements("span").ToList()[1].GetDirectInnerText();
                newClass.Town = childList[2].Elements("span").ToList()[3].GetDirectInnerText();
                newClass.State = childList[2].Elements("span").ToList()[5].GetDirectInnerText();
                newClass.ZipCode = childList[2].Elements("span").ToList()[6].GetDirectInnerText();
                newClass.Enrolled = int.Parse(childList[3].Element("b").Element("span")
                    .GetDirectInnerText().Split(", ")[0].Split(": ")[1]);
                newClass.MaxEnrolled = int.Parse(childList[3].Element("b").Element("span")
                    .GetDirectInnerText().Split(", ")[1].Split(": ")[1]);
                newClass.Cost = double.TryParse(childList[3].Element("b").Elements("span")
                    .ToList()[1].GetDirectInnerText().Split("$")[1], out var cost) ? cost : null;
                if (newClass.LocationName.ToLower().Contains("online"))
                {
                    newClass.IsInPerson = false;
                }
                else
                {
                    newClass.IsInPerson = true;
                }

                classes.Add(newClass);
            }
            catch (Exception e)
            {
                _logger.LogError($"An error occured while parsing a class: Exc: {e}");
                return null;
            }

        }
        return classes;
    }
}
