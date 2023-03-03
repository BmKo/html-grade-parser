using System.Net;
using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;
using HtmlGradeParser.Services;

namespace HtmlGradeParser.Parsers;

public abstract class AssessmentParser : IParser
{
    public static JsonNode Parse(HtmlNode node)
    {
        var header = node.SelectSingleNode("div/h4");
        var name = header?.SelectSingleNode("text()")?.InnerText.Trim();

        // TODO implement questions and check valid size
        var table = node.SelectSingleNode("div/div/table");
        var questions = table.Descendants("tr");
        var finalGrade = questions.LastOrDefault();

        var grades = GradeParser(finalGrade);

        return new JsonObject
        {
            ["name"] = name ?? "",
            ["letter_grade"] = grades.Item1,
            ["number_grade"] = grades.Item2
        };
    }

    private static (string, float) GradeParser(HtmlNode node)
    {
        var grade = WebUtility.HtmlDecode(node.SelectSingleNode("td/span/text()").InnerText);

        if (grade.Contains('/'))
        {
            var split = grade.Split('/').Select(x => x.Trim()).ToArray();
            if (split.Length != 2 || !float.TryParse(split[0], out var left) ||
                !float.TryParse(split[1], out var right))
            {
                throw new ArgumentException($"Invalid grade format: {grade}", nameof(node));
            }

            var numberGrade = MathF.Round(left / right * 100, 2);

            return (GradeConverterService.ConvertToLetter(numberGrade), numberGrade);
        }
        else
        {
            var numberGrade = GradeConverterService.ConvertToNumber(grade);
            return (grade, numberGrade);
        }
    }
}