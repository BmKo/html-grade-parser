using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using HtmlGradeParser.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
var headerPattern = new Regex(@"^(?<code>[A-Z]{4}\d{3})\s*-\s*(?<desc>.+?)\s*\((?<year>\d{4})\s+T(?<tri>[1-3])\)$");

app.MapPost("/ProcessGrades", ([FromBody] string compressedDocument) =>
{
    var document = GZipConverterService.GZipToHtml(compressedDocument);
    var json = ProcessHtml(document);

    return json;
});

JsonObject ProcessHtml(HtmlDocument document)
{
    var courseDivs = document.DocumentNode.Descendants("div")
        .Where(n => n.HasClass("tab-pane"));

    var courses = new JsonArray();
    foreach (var div in courseDivs)
    {
        courses.Add(ProcessCourse(div));
    }

    var jsonObject = new JsonObject { { "courses", new JsonArray { courses } } };
    return jsonObject;
}

JsonObject ProcessCourse(HtmlNode node)
{
    var header = node.SelectSingleNode("h3").InnerText;
    var match = headerPattern.Match(header);

    return new JsonObject
    {
        ["id"] = node.Id,
        ["courseCode"] = match.Groups["code"].Value,
        ["description"] = match.Groups["desc"].Value,
        ["trimester"] = int.Parse(match.Groups["tri"].Value),
        ["year"] = int.Parse(match.Groups["year"].Value)
    };
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();