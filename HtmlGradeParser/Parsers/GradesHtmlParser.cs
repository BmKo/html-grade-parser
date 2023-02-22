using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;

namespace HtmlGradeParser.Parsers;

public abstract class GradesHtmlParser : IParser
{
    public static JsonObject Parse(HtmlNode document)
    {
        var courseDivs = document.Descendants("div")
            .Where(n => n.HasClass("tab-pane"));

        var courses = new JsonArray();
        foreach (var div in courseDivs)
        {
            courses.Add(CourseParser.Parse(div));
        }

        var jsonObject = new JsonObject { { "courses", new JsonArray { courses } } };
        return jsonObject;
    }
}