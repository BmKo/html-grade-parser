using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;
using HtmlGradeParser.Services;

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

        string? letterGrade = null;

        if (float.TryParse(finalMark ?? "", out var numberGrade))
        {
            letterGrade = GradeConverterService.ConvertToLetter(numberGrade);
        }
        else if (finalMark != null)
        {
            letterGrade = finalMark;
            numberGrade = GradeConverterService.ConvertToNumber(finalMark);
        }

        return new JsonObject
        {
            ["id"] = div.Id,
            ["name"] = name ?? "",
            ["letter_grade"] = letterGrade,
            ["number_grade"] = numberGrade
        };
    }
}