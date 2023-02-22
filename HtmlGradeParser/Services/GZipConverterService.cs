using System.IO.Compression;
using HtmlAgilityPack;

namespace HtmlGradeParser.Services;

public static class GZipConverterService
{
    public static HtmlDocument ToHtml(string compressedDocument)
    {
        var compressedBytes = Convert.FromBase64String(compressedDocument);

        using var compressedStream = new MemoryStream(compressedBytes);
        using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var streamReader = new StreamReader(gzipStream);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(streamReader.ReadToEnd());

        return htmlDoc;
    }
}