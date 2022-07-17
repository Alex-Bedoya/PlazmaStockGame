using Microsoft.EntityFrameworkCore;
using PlazmaStockGame.Models;

namespace PlazmaStockGame.DataAccess
{
    public class StockDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //optionsBuilder.UseSqlServer(
            //   @"Server=DESKTOP-27VJS0V;Database=PlazmaStockGame;Trusted_Connection=True;");
            //Brendan
            //Ryan Desktop
            //@"Server=DESKTOP-D61Q4DT;Database=PlazmaStockGame;Trusted_Connection=True;");
            //Ryan Laptop LAPTOP-E5G80MMC
            //@"Server=LAPTOP-E5G80MMC;Database=PlazmaStockGame;Trusted_Connection=True;");


            //@"Server=DESKTOP-B6VT6G1\SQLEXPRESS;Database=PlazmaStockGame;Trusted_Connection=True;"); // alex

            //optionsBuilder.UseSqlServer("Server=localhost;Database=master;Trusted_Connection=True;"); //Adam
        }
    }
}
