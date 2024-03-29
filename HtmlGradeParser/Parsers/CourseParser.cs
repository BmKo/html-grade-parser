﻿using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlGradeParser.Parsers.Interfaces;

namespace HtmlGradeParser.Parsers;

public abstract class CourseParser : IParser
{
    private static readonly Regex HeaderPattern =
        new(@"^(?<code>[A-Z]{4}\d{3})\s*-\s*(?<desc>.+?)\s*\((?<year>\d{4})\s+T(?<tri>[1-3])\)$");

    public static JsonNode Parse(HtmlNode node)
    {
        var header = node.SelectSingleNode("h3").InnerText;
        var match = HeaderPattern.Match(header);

        var assessmentsNodes = node.Descendants("div")
            .Where(n => n.HasClass("panel"));

        var assessments = new JsonArray(assessmentsNodes.Select(AssessmentParser.Parse).ToArray());

        return new JsonObject
        {
            ["id"] = node.Id,
            ["courseCode"] = match.Groups["code"].Value,
            ["description"] = match.Groups["desc"].Value,
            ["trimester"] = int.Parse(match.Groups["tri"].Value),
            ["year"] = int.Parse(match.Groups["year"].Value),
            ["assessments"] = assessments
        };
    }
}