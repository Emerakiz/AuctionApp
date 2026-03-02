using AuctionApp.Data.Models;

namespace AuctionApp.Data.DTO
{
    public class BidListItemDTO
    {
        public int BidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidDate { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; } = "";
    }
}
