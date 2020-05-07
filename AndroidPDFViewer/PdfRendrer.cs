using Android.App;
using Android.OS;
using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics.Pdf;
using Android.Graphics;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;

namespace AndroidPDFViewer
{
    public class PdfRendrer
    {
        private Activity _context;
        public PdfRendrer(Activity context)
        {
            _context = context;
        }

        public async Task<List<Bitmap>> GetBitmapList(string url)
        {
            //check if(url is valid)
            string localFilePath = await DownloadFile(url);
            return ConverPdfToBitmap(localFilePath);
        }
        private async Task<string> DownloadFile(string webPdfUrl)
        {


            string pathToNewFolder = System.IO.Path.Combine(_context.CacheDir.AbsolutePath, "Pdf");
            string pathToFile = System.IO.Path.Combine(pathToNewFolder, System.IO.Path.GetFileName(webPdfUrl));
            Directory.CreateDirectory(pathToNewFolder);
            try
            {
                using (HttpClient cleint = new HttpClient())
                {
                    var response = await cleint.GetAsync(webPdfUrl);
                    response.EnsureSuccessStatusCode();
                    await using var content = await response.Content.ReadAsStreamAsync();
                    await using var file = File.Create(pathToFile);
                    content.Seek(0, SeekOrigin.Begin);
                    content.CopyTo(file);

                }
                return pathToFile;
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }

        private List<Bitmap> ConverPdfToBitmap(string filePath)
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            Java.IO.File file = new Java.IO.File(filePath);
            var fileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadOnly);
            var pdfRenderer = new PdfRenderer(fileDescriptor);
            for (int i = 0; i < pdfRenderer.PageCount; i++)
            {
                // Use the rendered bitmap.
                var page = pdfRenderer.OpenPage(i);
                var bitmap = Bitmap.CreateBitmap(page.Width, page.Height, Bitmap.Config.Argb8888);
                // Render the page to the bitmap.
                page.Render(bitmap, null, null, PdfRenderMode.ForDisplay);
                page.Close();
                bitmaps.Add(bitmap);
            }
            // Open the page to be rendered.
            // Close the page when you are done with it.
            pdfRenderer.Close();
            return bitmaps;

        }
    }
}