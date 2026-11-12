using System;
using System.ComponentModel.DataAnnotations;

namespace AssetTracking
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }

        public string Category { get; set; } = "";

        public string Brand { get; set; } = "";

        public string Model { get; set; } = "";

        public string Office { get; set; } = "";

        public DateTime PurchaseDate { get; set; }

        public decimal PriceUSD { get; set; }
    }
}