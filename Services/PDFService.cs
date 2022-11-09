
using System.Drawing;
using PFDLOCFINDER.Models;
using Spire.Pdf;
using Spire.Pdf.General.Find;

namespace PFDLOCFINDER.Services
{
    public interface IPDFService
    {
        Response<DocumentLocPageDto> getPDFLOC(IFormFile file, string textFind);
    }

    public class PDFService : IPDFService
    {
        [Obsolete]
        public Response<DocumentLocPageDto> getPDFLOC(IFormFile file, string textFind)
        {
            Response<DocumentLocPageDto> response = new Response<DocumentLocPageDto>();
            DocumentLocPageDto dataResponse = new DocumentLocPageDto();

            try
            {
                string filePath = Path.Combine(AppContext.BaseDirectory + @"\TEMP\", file.FileName);

                using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }

                PdfDocument doc = new PdfDocument();
                PointF p = new PointF();

                int pg;
                float szy, szx;
                string pagePDF, urx, ury, llx, lly;
                bool cek = false;

                szy = 0;
                szx = 0;
                pg = 0;
                cek = false;
                doc.LoadFromFile(filePath);

                PdfTextFind[] results = null;
                foreach (PdfPageBase page in doc.Pages)
                {
                    results = page.FindText(textFind).Finds;

                    SizeF size = page.Size / 2;
                    PdfTextFind text = results.FirstOrDefault();

                    if (text != null)
                    {
                        p = text.Position;
                        pg = text.SearchPageIndex;
                        cek = true;
                        szy = size.Height;
                        szx = size.Width;
                    }
                }

                if (cek)
                {
                    urx = (p.X - 20 + 50).ToString();
                    var uryd = (p.Y + 30 - 50);
                    llx = (p.X - 20).ToString();
                    var llyd = (p.Y + 30);

                    ury = uryd < szy ? (uryd + 2 * (szy - uryd)).ToString() : (uryd - 2 * (uryd - szy)).ToString();
                    lly = llyd < szy ? (llyd + 2 * (szy - llyd)).ToString() : (llyd - 2 * (llyd - szy)).ToString();

                    pagePDF = (pg + 1).ToString();
                }
                else
                {
                    urx = "0";
                    ury = "0";
                    llx = "0";
                    lly = "0";
                    pagePDF = "0";
                }

                dataResponse.urx = urx;
                dataResponse.ury = ury;
                dataResponse.llx = llx;
                dataResponse.lly = lly;
                dataResponse.pagePDF = pagePDF;

                response.http_response = 00;
                response.response_code = "200";
                response.response_message = "SUCCESS";

                DirectoryInfo directory = new DirectoryInfo(AppContext.BaseDirectory + @"\TEMP\");
                FileInfo[] FilesPdf = directory.GetFiles(file.FileName);
                foreach (FileInfo loopFile in FilesPdf) {
                     loopFile.Delete();
                }

            }
            catch (Exception ex)
            {
                dataResponse.urx = "0";
                dataResponse.ury = "0";
                dataResponse.llx = "0";
                dataResponse.lly = "0";
                dataResponse.pagePDF = "0";

                response.http_response = 99;
                response.response_code = "404";
                response.response_message = ex.Message;
            }

            response.data = dataResponse;

            return response;
        }
    }
}