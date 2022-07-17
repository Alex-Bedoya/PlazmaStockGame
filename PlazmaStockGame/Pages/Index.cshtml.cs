using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace PlazmaStockGame.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public const string SessionKeyTicker = "_Ticker";

        [BindProperty]
        [Required]
        public string Ticker { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            List<string> stocks = DataAccess.DbRepository.GetAllTickers();

            if (!ModelState.IsValid) { return Page(); }

            if (!stocks.Contains(Ticker))
            {
                ViewData["Ticker"] = string.Format("Ticker {0}, does not exist", Ticker);
                return Page(); 
            }

            HttpContext.Session.SetString(SessionKeyTicker, Ticker);

            return RedirectToPage("Game");
        }
    }
}