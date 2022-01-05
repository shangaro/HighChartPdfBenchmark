namespace PdfActor.Models;
public class PdfModel
{

    public string outputFile { get; set; }
    public string html{get;set;}

    public PdfModel(string html,string outputFile)
    {
        this.outputFile = outputFile;
        this.html=html;
    }
    public PdfModel SetOutputFile(string outputFile)
    {
        this.outputFile = outputFile;
        return this;
    }
}


