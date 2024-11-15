using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;

public class PdfGenerator
{
    private readonly IConverter _converter;

    public PdfGenerator(IConverter converter)
    {
        _converter = converter;
    }

    public byte[] GeneratePdf(string htmlContent)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                
            },
            Objects = {
                new ObjectSettings()
                {
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                },
            }
        };

        return _converter.Convert(doc);
    }
}