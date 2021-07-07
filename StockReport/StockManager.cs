using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockReport.AccountUtility;
using static StockReport.StockUtility;
using System.IO;
using Newtonsoft.Json;


namespace StockReport
{
    class StockManager
    {//Local variables
        int totalshare;
        LinkedList<string> timeOfTransaction = new LinkedList<string>();
        LinkedList<string> stockPurchased = new LinkedList<string>();
        LinkedList<string> stockSold = new LinkedList<string>();
        //Display Stock Details
        public void DisplayStocks(LinkedList<StockUtility.Stocks> stocksList)
        {
            Console.WriteLine("***********DISPLAYING STOCK DETAILS***************");
            foreach (var i in stocksList)
            {
                Console.WriteLine("\nStock name is: {0} \nStock share is: {1} \nStock Price is: {2}", i.StockName, i.shares, i.Price);
                int temp = i.shares * i.Price;
                totalshare += temp;
                Console.WriteLine("Total stock price for {0} is : {1}", i.StockName, temp);
            }
            Console.WriteLine("Total store is : {0}", totalshare);

        }
        //Add a stock
        public void AddStock(LinkedList<StockUtility.Stocks> stocks)
        {
            Stocks stocks1 = new Stocks();
            Console.WriteLine("Enter the stock name: ");
            stocks1.StockName = Console.ReadLine();
            Console.WriteLine("Enter number of shares ");
            stocks1.shares = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the stock price: ");
            stocks1.Price = Convert.ToInt32(Console.ReadLine());
            stocks.AddLast(stocks1);
        }
        //Delete a stock using linked list
        public void DeleteInventory(LinkedList<StockUtility.Stocks> stocks)
        {
            Console.WriteLine("Enter the stock name to be deleted: ");
            var temp = Console.ReadLine();
            var node = stocks.First;
            while (node != null)
            {
                var nextNode = node.Next;
                if (node.Value.StockName == temp)
                {
                    stocks.Remove(node);
                }
                node = nextNode;
            }
        }

        public void DisplayAccount(List<AccountUtility.Account> AccountList)
        {
            Console.WriteLine("***********DISPLAYING ACCOUNT DETAILS***************");
            foreach (var i in AccountList)
            {
                Console.WriteLine("\nStock holder {0}", i.Stockholder);
                Console.WriteLine("Stock name is: {0} \nStock share is: {1} \nStock Price is: {2}", i.StockName, i.shares, i.Price);
                int temp = i.shares * i.Price;
                totalshare += temp;
                Console.WriteLine("Total stock price for {0} is : {1}", i.StockName, temp);
            }
            Console.WriteLine("Total stock share is : {0}", totalshare);

        }
        //Account details are printed
        public void StockAccount(String acc)
        {

            AccountUtility accountUtility = JsonConvert.DeserializeObject<AccountUtility>(File.ReadAllText(acc));
            var fs1 = accountUtility.AccountList;
            DisplayAccount(fs1);

        }
        //Method to buy a stock
        public void Buy(int amount, string company)
        {
            string acc = @"C:\Users\DELL\Desktop\StockMarket\StockMarket\account.json";
            AccountUtility accountUtility = JsonConvert.DeserializeObject<AccountUtility>(File.ReadAllText(acc));
            var fs1 = accountUtility.AccountList;
            AddStockAccount(fs1, company, amount);
            File.WriteAllText(acc, JsonConvert.SerializeObject(accountUtility));
            stockPurchased.AddLast("Company: " + company + " Amount: " + amount);
            DisplayAccount(fs1);
        }
        //Method to sell a stock
        public void Sell(int amount, string company)
        {
            string acc = @"C:\Users\DELL\Desktop\StockMarket\StockMarket\account.json";
            AccountUtility accountUtility = JsonConvert.DeserializeObject<AccountUtility>(File.ReadAllText(acc));
            var fs1 = accountUtility.AccountList;
            SellStockAccount(fs1, company, amount);
            File.WriteAllText(acc, JsonConvert.SerializeObject(accountUtility));
            stockSold.AddLast("Company: " + company + " Amount: " + amount);
            DisplayAccount(fs1);
        }
        //Perform sell operation on Account
        public void SellStockAccount(List<AccountUtility.Account> accountlist, string company, int amount)
        {
            string file = @"C:\Users\DELL\Desktop\StockMarket\StockMarket\Json.json";
            string acc = @"C:\Users\DELL\Desktop\StockMarket\StockMarket\account.json";
            AccountUtility accountUtility = JsonConvert.DeserializeObject<AccountUtility>(File.ReadAllText(acc));
            StockUtility stockUtility = JsonConvert.DeserializeObject<StockUtility>(File.ReadAllText(file));
            var fs = stockUtility.stocksList;
            foreach (var stockavail in fs)
            {
                if (stockavail.StockName == company && stockavail.shares >= 0)
                {
                    foreach (var member in accountlist)
                    {

                        Account account1 = new Account();

                        if (member.StockName == company && member.shares >= 1)
                        {
                            Console.WriteLine("Updated!");

                            Console.WriteLine("\nEnter the stock holder name: ");
                            account1.Stockholder = Console.ReadLine();
                            account1.StockName = company;
                            account1.shares = member.shares - 1;
                            account1.Price = amount;
                            accountlist.Remove(member);

                            stockavail.shares += 1;
                            accountlist.Add(account1);
                            File.WriteAllText(file, JsonConvert.SerializeObject(stockUtility));
                            DateTime time = DateTime.Now;
                            Console.WriteLine("Sold the Share at: " + time);
                            timeOfTransaction.AddFirst("Sold company " + company + " at time " + Convert.ToString(time));

                            break;
                        }
                    }
                }
            }
        }
        //Perform buy operation on Account
        public void AddStockAccount(List<AccountUtility.Account> accountlist, string company, int amount)
        {
            string file = @"C:\Users\DELL\Desktop\StockMarket\StockMarket\Json.json";
            string acc = @"C:\Users\DELL\Desktop\StockMarket\StockMarket\account.json";
            AccountUtility accountUtility = JsonConvert.DeserializeObject<AccountUtility>(File.ReadAllText(acc));
            StockUtility stockUtility = JsonConvert.DeserializeObject<StockUtility>(File.ReadAllText(file));
            var fs = stockUtility.stocksList;
            int flag = 0;
            int present = 0;

            foreach (var stockavail in fs)
            {
                if (stockavail.StockName == company && stockavail.shares >= 1)
                {
                    flag = 1;
                    foreach (var member in accountlist)
                    {

                        Account account1 = new Account();

                        if (member.StockName == company)
                        {
                            Console.WriteLine("Updated");

                            Console.WriteLine("Enter the stock holder name: ");
                            account1.Stockholder = Console.ReadLine();
                            account1.StockName = company;
                            account1.shares = member.shares + 1;
                            account1.Price = amount;
                            accountlist.Remove(member);
                            stockavail.shares -= 1;
                            accountlist.Add(account1);
                            DateTime time = DateTime.Now;
                            Console.WriteLine("Bought the Share at: " + time);
                            timeOfTransaction.AddFirst("Bought company " + company + " at time " + Convert.ToString(time));
                            File.WriteAllText(file, JsonConvert.SerializeObject(stockUtility));
                            present = 1;
                            break;
                        }
                    }

                }
                if (flag == 1 && present == 0)
                {
                    Account account1 = new Account();
                    Console.WriteLine("Enter the stock holder name: ");
                    account1.Stockholder = Console.ReadLine();
                    account1.StockName = company;
                    account1.shares = 1;
                    account1.Price = amount;
                    stockavail.shares -= 1;
                    accountlist.Add(account1);
                    DateTime time = DateTime.Now;
                    Console.WriteLine("Bought the Share at: " + time);
                    timeOfTransaction.AddFirst("Bought company " + company + " at time " + Convert.ToString(time));
                    File.WriteAllText(file, JsonConvert.SerializeObject(stockUtility));
                    break;
                }

            }
            if (flag == 0)
            {
                Console.WriteLine("Stocks not available");
            }

        }
        //To print date and time of transaction
        public void DateandTime()
        {
            Console.WriteLine("\n--------------The Date and Time of transactions--------------\n");
            foreach (var time in timeOfTransaction)
            {
                Console.WriteLine(time);
            }
        }
        public void StockPurchased()
        {
            Console.WriteLine("\n--------------List of stock purchased--------------\n");
            foreach (var time in stockPurchased)
            {
                Console.WriteLine(time);
            }
        }
        public void StockSold()
        {
            Console.WriteLine("\n--------------List of stock sold--------------\n");
            foreach (var time in stockSold)
            {
                Console.WriteLine(time);
            }
        }
    }
}
