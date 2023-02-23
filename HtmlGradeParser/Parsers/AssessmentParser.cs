using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;

namespace HtmlGradeParser.Parsers;

public abstract class AssessmentParser : IParser
{
    public static JsonNode Parse(HtmlNode node)
    {
        var div = node.SelectSingleNode("div");
        var header = div.SelectSingleNode("h4");

        var name = header?.SelectSingleNode("text()")?.InnerText.Trim();
        var finalMark = header?.SelectSingleNode("span/text()")?
            .InnerText.Split(':').LastOrDefault()?.Trim();
        
        return new JsonObject
        {
            ["id"] = div.Id,
            ["name"] = name ?? "",
            ["final_mark"] = finalMark ?? ""
        };
    }
}