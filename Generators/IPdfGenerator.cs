namespace Notification.HighCharPdfBenchmark;

public interface IPdfGenerator
{
    Task GeneratePdf(string html, string pdfPath);
    Task Init();
}

