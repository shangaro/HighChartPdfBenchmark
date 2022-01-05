using System.Diagnostics;
using IronPdf;
using PdfActor.Models;

namespace Notification.HighCharPdfBenchmark;

public class IronPdfGen
{
    private ChromePdfRenderer _renderer;

    public IronPdfGen()
    {
    }

    public async Task GeneratePdf(string html, string pdfPath)
    {


        _renderer.RenderingOptions.FitToPaper = true;
        _renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Print;
        _renderer.RenderingOptions.EnableJavaScript = true;
        _renderer.RenderingOptions.PrintHtmlBackgrounds = true;
        _renderer.RenderingOptions.ApplyMarginToHeaderAndFooter = true;
        _renderer.RenderingOptions.MarginTop = 10d;
        _renderer.RenderingOptions.MarginBottom = 10d;
        _renderer.RenderingOptions.MarginLeft = 20d;
        _renderer.RenderingOptions.MarginRight = 20d;
        // Renderer.RenderingOptions.CreatePdfFormsFromHtml = true;
        var doc = await _renderer.RenderHtmlAsPdfAsync(html);
        //var doc = Renderer.RenderUrlAsPdf("https://www.google.com/");
        // Renderer.RenderingOptions.HtmlHeader = new HtmlHeaderFooter()
        // {
        // };

        // doc.SaveAs(pdfPath);       

    }

    public void LaunchRenderer()
    {
        _renderer = new ChromePdfRenderer();

    }
}