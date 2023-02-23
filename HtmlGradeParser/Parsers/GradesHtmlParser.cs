using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;

namespace HtmlGradeParser.Parsers;

public abstract class GradesHtmlParser : IParser
{
    public static JsonNode Parse(HtmlNode document)
    {
        var courseDivs = document.Descendants("div")
            .Where(n => n.HasClass("tab-pane"));

        var courses = new JsonArray(courseDivs.Select(CourseParser.Parse).ToArray());

        return new JsonObject { { "courses", courses } };
    }
}