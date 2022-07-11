using PlazmaStockGame.Models;

using Microsoft.AspNetCore.Mvc;

using System.Security.Cryptography;

namespace PlazmaStockGame.DataAccess
{
    public class DbRepository
    {
        /// <summary>
        /// Gets a list of all stocks with a specific Ticker from the database
        /// </summary>
        /// <param name="tick">the ticker symbol EX: TSLA</param>
        /// <returns>a list of stock objects for 1 ticker</returns>
        public static List<Stock> GetAllStocksByTicker(string tick)
        {
            List<Stock> stocks;
            try
            {
                using (StockDbContext db = new StockDbContext())
                {
                    stocks = db.Stocks.Where(q => q.Ticker.Equals(tick)).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return stocks;

        }

        /// <summary>
        /// Gets a single stock based on the ticker and date
        /// </summary>
        /// <param name="tick">ticker symbol EX: TSLA</param>
        /// <param name="day">Specific Date for a specific symbol</param>
        /// <returns>returns a list of stocks, but should only be one item in the list</returns>
        public static List<Stock> GetStockByDate(string tick, DateOnly day)
        {
            List<Stock> stocks;
            try
            {
                using (StockDbContext db = new StockDbContext())
                {
                    stocks = db.Stocks.Where(q => q.Ticker.Equals(tick) && q.Date.Equals(day)).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return stocks;
        }
    }
}
