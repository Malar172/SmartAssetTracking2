using AssetTracking;
using System.Linq;

namespace AssetTracking
{
    internal class Program
    {
        static AssetDbContext context = new AssetDbContext();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("=================================");
                Console.WriteLine(" SMART ASSET TRACKING SYSTEM");
                Console.WriteLine("=================================");
                Console.WriteLine("1. Add Asset");
                Console.WriteLine("2. Show Assets");
                Console.WriteLine("3. Update Asset");
                Console.WriteLine("4. Delete Asset");
                Console.WriteLine("5. Search Asset");
                Console.WriteLine("6. Exit");

                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAsset();
                        break;

                    case "2":
                        ShowAssets();
                        break;

                    case "3":
                        UpdateAsset();
                        break;

                    case "4":
                        DeleteAsset();
                        break;

                    case "5":
                        SearchAsset();
                        break;

                    case "6":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid Option");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddAsset()
        {
            Console.Clear();

            Console.Write("Category: ");
            string category = Console.ReadLine() ?? "";

            Console.Write("Brand: ");
            string brand = Console.ReadLine() ?? "";

            Console.Write("Model: ");
            string model = Console.ReadLine() ?? "";

            Console.Write("Office: ");
            string office = Console.ReadLine() ?? "";

            Console.Write("Purchase Date (yyyy-mm-dd): ");

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime purchaseDate))
            {
                Console.WriteLine("Invalid Date");
                Console.ReadKey();
                return;
            }

            Console.Write("Price USD: ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Invalid Price");
                Console.ReadKey();
                return;
            }

            Asset asset = new Asset
            {
                Category = category,
                Brand = brand,
                Model = model,
                Office = office,
                PurchaseDate = purchaseDate,
                PriceUSD = price
            };

            context.Assets.Add(asset);
            context.SaveChanges();

            Console.WriteLine("\nAsset Added Successfully!");
            Console.ReadKey();
        }

        static string GetStatus(DateTime purchaseDate)
        {
            DateTime endDate = purchaseDate.AddYears(3);

            double monthsLeft =
                (endDate - DateTime.Now).TotalDays / 30;

            if (monthsLeft < 3)
                return "RED";

            if (monthsLeft < 6)
                return "YELLOW";

            return "NORMAL";
        }

        static void ShowAssets()
        {
            Console.Clear();

            var assets = context.Assets
                .OrderBy(a => a.Category)
                .ThenBy(a => a.PurchaseDate)
                .ToList();

            Console.WriteLine(
                $"{"ID",-4}" +
                $"{"CATEGORY",-13}" +
                $"{"BRAND",-12}" +
                $"{"MODEL",-15}" +
                $"STATUS");

            Console.WriteLine(new string('-', 55));

            foreach (var asset in assets)
            {
                string status = GetStatus(asset.PurchaseDate);

                Console.Write(
                    $"{asset.Id,-4}" +
                    $"{asset.Category,-13}" +
                    $"{asset.Brand,-12}" +
                    $"{asset.Model,-15}"
                );

                if (status == "RED")
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (status == "YELLOW")
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(status);

                Console.ResetColor();
            }

            Console.ReadKey();
        }

        static void UpdateAsset()
        {
            Console.Clear();

            Console.Write("Enter Asset ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID");
                Console.ReadKey();
                return;
            }

            var asset = context.Assets
                .FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset Not Found");
                Console.ReadKey();
                return;
            }

            Console.Write("New Brand: ");
            asset.Brand = Console.ReadLine() ?? "";

            Console.Write("New Model: ");
            asset.Model = Console.ReadLine() ?? "";

            context.SaveChanges();

            Console.WriteLine("Asset Updated!");
            Console.ReadKey();
        }

        static void DeleteAsset()
        {
            Console.Clear();

            Console.Write("Enter Asset ID: ");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID");
                Console.ReadKey();
                return;
            }

            var asset = context.Assets
                .FirstOrDefault(a => a.Id == id);

            if (asset == null)
            {
                Console.WriteLine("Asset Not Found");
                Console.ReadKey();
                return;
            }

            context.Assets.Remove(asset);
            context.SaveChanges();

            Console.WriteLine("Asset Deleted!");
            Console.ReadKey();
        }

        static void SearchAsset()
        {
            Console.Clear();

            Console.Write("Enter Brand: ");
            string brand = Console.ReadLine() ?? "";

            var results = context.Assets
                .Where(a => a.Brand.Contains(brand))
                .ToList();

            foreach (var asset in results)
            {
                Console.WriteLine(
                    $"{asset.Id} | {asset.Brand} | {asset.Model}");
            }

            Console.ReadKey();
        }
    }
}