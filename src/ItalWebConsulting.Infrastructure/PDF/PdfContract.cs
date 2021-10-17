using System;
using System.Collections.Generic;
using System.Text;

namespace ItalWebConsulting.Infrastructure.PDF
{
  public  class PdfContract
    {
        //text that you want to display on pdf
        public string Text { get; set; }
        //Image that you want to display on pdf , the path must be from your pc
        public string ImagePath { get; set; }
        // path of your pdf file 
        public string FilePath { get; set; }
        //name of your file
        public string FileName { get; set; }
    }
}
