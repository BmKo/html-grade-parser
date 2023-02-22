using HtmlGradeParser.Services;
using HtmlGradeParser.Parsers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

app.MapPost("/ProcessGrades", ([FromBody] string compressedDocument) =>
{
    var document = GZipConverterService.ToHtml(compressedDocument);
    return GradesHtmlParser.Parse(document.DocumentNode);
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();