using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlazmaStockGame.Pages
{
    public class GameModel : PageModel
    {
        public string Ticker { get; set; }
        public void OnGet()
        {
            Ticker = HttpContext.Session.GetString("_Ticker");
        }
    }
}
