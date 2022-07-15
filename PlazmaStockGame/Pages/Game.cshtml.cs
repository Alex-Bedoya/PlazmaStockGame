using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlazmaStockGame.Models;
using PlazmaStockGame.DataAccess;

namespace PlazmaStockGame.Pages
{
    public class GameModel : PageModel
    {
        public string Ticker { get; set; }

        public double CurrMoney { get; set; }

        public DateTime StartDate { get; set; }

        public List<Stock> stocks { get; set; }

        public int CurrDayIndex { get; set; }

        public int StocksOwned { get; set; }





        public void OnGet()
        {
            Ticker = HttpContext.Session.GetString("_Ticker");
            StartDate = RandomDay();
            CurrDayIndex = 0;
            CurrMoney = 10000;
            StocksOwned = 0;
            stocks = DbRepository.GetStocksAfterDate(Ticker, StartDate);

            PushData();//push all the data to the session
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
        /// function to get all the data into the class variables from the Session.
        /// We do this because the Variables become null when called from Ajax
        /// </summary>
        public void PullData()
        {
            Ticker = HttpContext.Session.GetString("_Ticker");
            CurrMoney = Double.Parse(HttpContext.Session.GetString("_CurrMoney"));
            StartDate = DateTime.Parse(HttpContext.Session.GetString("_StartDate"));
            CurrDayIndex = Convert.ToInt32(HttpContext.Session.GetInt32("_CurrDayIndex"));
            StocksOwned = Convert.ToInt32(HttpContext.Session.GetInt32("_StocksOwned"));
            stocks = DbRepository.GetStocksAfterDate(Ticker, StartDate);
        }

        /// <summary>
        /// functions that pushes all the class data into the Session to be pulled out later
        /// </summary>
        public void PushData()
        {
            HttpContext.Session.SetString("_Ticker", Ticker);
            HttpContext.Session.SetString("_CurrMoney", CurrMoney.ToString());
            HttpContext.Session.SetInt32("_CurrDayIndex", CurrDayIndex);
            HttpContext.Session.SetInt32("_StocksOwned", StocksOwned);
            HttpContext.Session.SetString("_StartDate", StartDate.ToString());
        }


        /// <summary>
        /// method for when the user wants to buy stocks for that day.
        /// </summary>
        /// <param name="NumStocksBuying">The number of stocks that the user is buying</param>
        public void Buy(int NumStocksBuying)
        {
            PullData();

            double costToBuy = stocks[CurrDayIndex].Cost * NumStocksBuying;

            if (costToBuy < CurrMoney)
            {
                //spend the money to buy stocks, add the number of stocks they bought, increment day
                CurrMoney = CurrMoney - costToBuy;
                StocksOwned = StocksOwned + NumStocksBuying;
                CurrDayIndex++;
            }
            else
            {
                //you don't have enough money to buy stocks
            }

            if (CurrDayIndex >= 7)
            {
                //the game is over because the game lasts for 7 days
                Quit();
            }


            PushData();
        }


        /// <summary>
        /// Method for when the user wants to sell any of the stocks they currently own
        /// </summary>
        /// <param name="NumStocksSelling">the number of stocks the user wants to sell</param>
        public void Sell(int NumStocksSelling)
        {
            PullData();

            if (NumStocksSelling <= 0)
            {
                //you cant sell what you don't have
                return;
            }

            double SellAmount = stocks[CurrDayIndex].Cost * NumStocksSelling;

            if (NumStocksSelling <= StocksOwned)
            {
                //add the money to the current total, subtract the amount of stocks they own
                CurrMoney = CurrMoney + SellAmount;
                StocksOwned = StocksOwned - NumStocksSelling;
                CurrDayIndex++;
            }
            else
            {
                //error, you don't have that many stocks to sell
                
            }

            if (CurrDayIndex >= 7)
            {
                //the game is over because the game lasts for 7 days
                Quit();
            }


            PushData();
        }


        /// <summary>
        /// method to pass to the next day without doing anything to the other data.
        /// </summary>
        public void Hold()
        {
            PullData();

            if (CurrDayIndex >= 7)
            {
                //the game is over because the game lasts for 7 days
                Quit();
                PushData();
                return;
            }

            CurrDayIndex++;

            PushData();
        }


        /// <summary>
        /// method to sell all the stocks the user currently owns and ends the game.
        /// </summary>
        public void Quit()
        {
            PullData();

            double sellMoney = stocks[CurrDayIndex].Cost * StocksOwned;
            StocksOwned = 0;

            CurrMoney = CurrMoney + sellMoney;

            //set it to above 7 (the max days you can play the game for)
            CurrDayIndex = 8;



            PushData();


            //do thing to quit the game and show the results
            //...
        }






        //Ajax functions

        public IActionResult OnPostNextDay(DateTime date)
        {
            //CurrDate = date;
            //NextDay();
            //return new JsonResult(CurrDate);
            return new JsonResult(date);//this is fake line
        }

        public IActionResult OnPostPurchase(int amount, int option)
        {
            if (option == 0)
            {
                //if the option they selected was buy
                Buy(amount);
            }
            else if (option == 1)
            {
                //if the option they selected was sell
                Sell(amount);
            }
            else if (option == 2)
            {
                //if the option they selected was hold
                Hold();
            }





            DataPackage data = new DataPackage(CurrMoney, stocks, CurrDayIndex, StocksOwned);
            JsonResult json = new JsonResult(data);
            return json;
        }

    }


    /// <summary>
    /// this object was made to package all the data we need to send back using a JsonResult. 
    /// </summary>
    public class DataPackage {

        public double CurrMoney { get; set; }

        public List<Stock> stocks { get; set; }

        public int CurrDayIndex { get; set; }

        public int StocksOwned { get; set; }

        public DataPackage( double currMoney,  List<Stock> stocks, int currDayIndex, int stocksOwned)
        {
            CurrMoney = currMoney;
            this.stocks = stocks;
            CurrDayIndex = currDayIndex;
            StocksOwned = stocksOwned;
        }
    }

}
