using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlazmaStockGame.Pages
{
    public class GameModel : PageModel
    {
        public string Ticker { get; set; }

        public double CurrMoney { get; set; }

        public DateTime CurrDate { get; set; }



       


        public void OnGet()
        {
            Ticker = HttpContext.Session.GetString("_Ticker");
            CurrDate = RandomDay();
            CurrMoney = 10000;
        }


        
        /// <summary>
        /// Generates a Random day within the range of the stocks we picked
        /// </summary>
        /// <returns>Returns the DateTime or day that we start the game at</returns>
        public DateTime RandomDay()
        {
            Random gen = new Random();
            DateTime start = new DateTime(2012, 5, 1);
            DateTime end = new DateTime(2012, 12, 31);

            //we divide by 2 so that the max we can start at is half way through, we don't want them startign on the last day
            int range = (end - start).Days / 2; 
            return start.AddDays(gen.Next(range));
        }

        /// <summary>
        /// Increments the Day by 1 so that the user can do a different day
        /// </summary>
        public void NextDay()
        {
            CurrDate = CurrDate.AddDays(1);
        }


        public IActionResult OnPostNextDay(DateTime date)
        {
            CurrDate = date;
            NextDay();
            return new JsonResult(CurrDate);
        }

        public IActionResult OnPostPurchase(int amount)
        {
            return new JsonResult("some stuff");
        }

    }
}
