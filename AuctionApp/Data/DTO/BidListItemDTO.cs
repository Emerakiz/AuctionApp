using AuctionApp.Data.Models;

namespace AuctionApp.Data.DTO
{
    public class BidListItemDTO
    {
        public decimal Amount { get; set; }
        public DateTime TimePlaced { get; set; }
        public string Name { get; set; } = "";
    }
}
