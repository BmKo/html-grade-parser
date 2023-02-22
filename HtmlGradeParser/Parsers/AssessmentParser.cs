using System.Text.Json.Nodes;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;

namespace HtmlGradeParser.Parsers;

public abstract class AssessmentParser : IParser
{
    public static JsonObject Parse(HtmlNode node)
    {
        throw new NotImplementedException();
    }
}