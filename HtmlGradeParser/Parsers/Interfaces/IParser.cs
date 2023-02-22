using System.Text.Json.Nodes;
using HtmlAgilityPack;

namespace HtmlGradeParser.Parsers.Interfaces;

public interface IParser
{
    public static abstract JsonObject Parse(HtmlNode node);
}