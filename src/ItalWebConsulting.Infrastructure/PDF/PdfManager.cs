using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Text;
using PdfSharpCore.Drawing;
namespace ItalWebConsulting.Infrastructure.PDF
{
   public  class PdfManager
    {
        public void CreatePDF(PdfContract input)
        {
            using (PdfDocument document = new PdfDocument())
            {


                var page = document.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("OpenSans", 20, XFontStyle.Bold);

                gfx.DrawString(input.Text, font, XBrushes.Black, new XRect(20, 20, page.Width, page.Height), XStringFormats.TopCenter);
                gfx.DrawImage(XImage.FromFile(input.ImagePath), 5, 100);
                document.Save(input.FilePath + input.FileName);

            }
        }

    }
}
