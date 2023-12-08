using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lab4.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<string> Images { get; set; }
        private string imagesDir;
        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            imagesDir = Path.Combine(environment.WebRootPath, "images");
        }

        public void OnGet()
        {
            UpdateFileList();
        }

        private void UpdateFileList()
        {
            Images = new List<string>();
            foreach (var item in Directory.EnumerateFiles(imagesDir).ToList())
            {
                Images.Add(Path.GetFileName(item));
            }
        }
    }

}