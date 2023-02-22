using System.IO.Compression;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var app = builder.Build();
var headerPattern = new Regex(@"^(?<code>[A-Z]{4}\d{3})\s*-\s*(?<desc>.+?)\s*\((?<year>\d{4})\s+T(?<tri>[1-3])\)$");

app.MapPost("/ProcessGrades", ([FromBody] string compressedDocument) =>
{
    var document = GZipToHtml(compressedDocument);
    var json = ProcessHtml(document);

    return Task.FromResult(json);
});

static HtmlDocument GZipToHtml(string compressedDocument)
{
    var compressedBytes = Convert.FromBase64String(compressedDocument);

    using var compressedStream = new MemoryStream(compressedBytes);
    using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
    using var streamReader = new StreamReader(gzipStream);

    var htmlDoc = new HtmlDocument();
    htmlDoc.LoadHtml(streamReader.ReadToEnd());

    return htmlDoc;
}

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
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();