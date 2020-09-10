using FirmaElectronica_KeyNUA.Model;
using iTextSharp.text.pdf;
using System.IO;

namespace FirmaElectronica_KeyNUA.Util
{
    public class GeneratePDF
    {
        public GeneratePDF()
        {

        }
        public string GenerateInvestorDocument(InvestorInfo investorInfo)
        {
            string fullName = string.Concat(investorInfo.FirstName, " ", investorInfo.LastName);

            string filePath = @"Template_Investor\";

            string fileNameExisting = @"TemplateInvestor.pdf";
            string fileNameNew = @"InvestorContract_" + fullName.Replace(" ","").Trim() + ".pdf";

            string fullNewPath = filePath + fileNameNew;
            string fullExistingPath = filePath + fileNameExisting;

            using (var existingFileStream = new FileStream(fullExistingPath, FileMode.Open))

            using (var newFileStream = new FileStream(fullNewPath, FileMode.Create))
            {
                // Open existing PDF
                var pdfReader = new PdfReader(existingFileStream);

                // PdfStamper, which will create
                var stamper = new PdfStamper(pdfReader, newFileStream);

                AcroFields fields = stamper.AcroFields;
                fields.SetField("FULLNAME", fullName);
                fields.SetField("DOCNUMBER", investorInfo.DocumentNumber);
                fields.SetField("RUCNUMBER", investorInfo.RUCNumber);
                fields.SetField("FULLADDRESS", investorInfo.FullAddress);
                fields.SetField("SIGNATUREFULLNAME", fullName);
                fields.SetField("SIGNATUREDOCNUMBER", investorInfo.DocumentNumber);
                fields.SetField("SIGNATURERUCNUMBER", investorInfo.RUCNumber);

                // "Flatten" the form so it wont be editable/usable anymore
                stamper.FormFlattening = true;

                stamper.Close();
                pdfReader.Close();

                //return new FileStreamResult(newFileStream, "application/pdf");
                return fileNameNew;
            }
        }
    }
}
