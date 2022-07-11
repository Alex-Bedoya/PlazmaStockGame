using System.ComponentModel.DataAnnotations;

namespace PlazmaStockGame.Models
{
    public class Stock
    {
        [Key]
        public int ID { get; set; }
        public string Ticker { get; set; }
        public DateTime Date  { get; set; }
        public double Cost { get; set; }

        public Stock(string ticker, DateTime date, double cost)
        {
            Ticker = ticker;
            Date = date;
            Cost = cost;
        }
    }
}
