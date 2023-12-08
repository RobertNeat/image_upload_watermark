using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Lab4.Pages
{
    public class UploadModel : PageModel
    {
        [BindProperty]
        public IFormFile Upload { get; set; }
        private string imagesDir;
        private MagickImage watermark;

        public UploadModel(IWebHostEnvironment environment)
        {
            imagesDir = Path.Combine(environment.WebRootPath, "images");

            watermark = new MagickImage("watermark.png");
            watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 2);
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Upload != null)
            {
                string extension = ".jpg";
                switch (Upload.ContentType)
                {
                    case "image/png":
                        extension = ".png";
                        break;
                    case "image/gif":
                        extension = ".gif";
                        break;
                }

                var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
                var filePath = Path.Combine(imagesDir, fileName);

                //using (var fs = System.IO.File.OpenWrite(filePath))
                //{
                //    await Upload.CopyToAsync(fs);
                //}

                // Add watermark to the uploaded image
                using (var image = new MagickImage(Upload.OpenReadStream()))//filePath
                {
                    image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);
                    await image.WriteAsync(filePath);
                }
            }

            return RedirectToPage("Index");
        }
    }
}
