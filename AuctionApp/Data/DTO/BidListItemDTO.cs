using AuctionApp.Data.Models;

namespace AuctionApp.Data.DTO
{
    public class BidListItemDTO
    {
        public int BidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
        public string Name { get; set; } = "";
    }
}
