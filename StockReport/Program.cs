using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace StockReport
{
    class Program
    {
        static void Main(string[] args)
        {


            //Creating obj for StockManager
            StockManager stockManager = new StockManager();
            //getting path of json file
            string file = @"C:\Users\HP\source\repos\StockReport\StockReport\Json.json";
            string acc = @"C:\Users\HP\source\repos\StockReport\StockReport\Account.json";
            //DeserializeO Json file
            StockUtility stockUtility = JsonConvert.DeserializeObject<StockUtility>(File.ReadAllText(file));

            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("                            STOCK MANAGEMENT METHODS                                 ");
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("Enter which operation to perform on Stock Inventory\n 1-Add a stock\n 2-Remove a stock\n 3-Display Stocks");

            AccountUtility accountUtility = JsonConvert.DeserializeObject<AccountUtility>(File.ReadAllText(acc));
            int num = Convert.ToInt32(Console.ReadLine());
            var fs = stockUtility.stocksList;
            //Performing stock management functions
            switch (num)
            {
                case 1:
                    stockManager.AddStock(fs);
                    File.WriteAllText(file, JsonConvert.SerializeObject(stockUtility));
                    stockManager.DisplayStocks(fs);
                    break;
                case 2:
                    stockManager.DeleteInventory(fs);
                    File.WriteAllText(file, JsonConvert.SerializeObject(stockUtility));
                    stockManager.DisplayStocks(fs);
                    break;
                case 3:
                    stockManager.DisplayStocks(fs);
                    break;
            }
            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("                            STOCK ACCOUNT METHODS                                    ");
            Console.WriteLine("-------------------------------------------------------------------------------------");
            //Perform account management function
            string flag = "Y";
            while (flag == "Y")
            {
                Console.WriteLine("Please Enter :\n1-Display user account\n2-To buy a share\n3-To sell a share\n4-To Display Account report");
                int ch = Convert.ToInt32(Console.ReadLine());
                var fs1 = accountUtility.AccountList;
                switch (ch)
                {
                    case 1:
                        stockManager.StockAccount(acc);
                        break;
                    case 2:
                        Console.WriteLine("Enter amount: ");
                        int amount = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter company name in which you want to buy share: ");
                        string companyname = Console.ReadLine();
                        stockManager.Buy(amount, companyname);


                        break;
                    case 3:
                        Console.WriteLine("Enter amount: ");
                        int amount1 = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter company name in which you want to sell share: ");
                        string companyname1 = Console.ReadLine();
                        stockManager.Sell(amount1, companyname1);

                        break;
                    case 4:
                        stockManager.StockPurchased();
                        stockManager.StockSold();
                        stockManager.DateandTime();
                        break;


                }
                Console.WriteLine("\nDo you want to continue?(Y/N)");
                flag = Console.ReadLine();
            }


        }
    }
}
