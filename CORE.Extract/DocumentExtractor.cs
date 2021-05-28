using System;
using GemBox.Pdf;

namespace CORE.Extract
{
    public class DocumentExtractor
    {
        public DocumentExtractor()
        {
            
        }
        
        public static string Extract(string filename)
        {
            ComponentInfo.SetLicense("AN-2020Aug05-VFcnyp8mWvXcdMtN3yMl+jcaXITJLh8hqcgKSf8Yd40TVwiuabgvjc8bFMq11fNccJwpNKjKTeNPzJV6VikoPCo211g==A");
            var content = "";
            try
            {
                // Iterate through PDF pages and extract each page's Unicode text content.
                using (var document = PdfDocument.Load(filename))
                {
                    foreach (var page in document.Pages)
                    {
                        content += page.Content.ToString() + Environment.NewLine;
                        Console.WriteLine(page.Content.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return content;
        }
    }
}