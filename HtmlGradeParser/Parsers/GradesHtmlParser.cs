using System.Text.Json.Nodes;
using HtmlAgilityPack;

namespace HtmlGradeParser.Parsers;

public static class GradesHtmlParser
{
    public static JsonObject Parse(HtmlDocument document)
    {
        var courseDivs = document.DocumentNode.Descendants("div")
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