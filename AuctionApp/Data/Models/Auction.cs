namespace AuctionApp.Data.Models
{
    public class Auction
    {
        public int AuctionId { get; set; }

        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal StartingPrice { get; set; }
        public decimal? CurrentPrice { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Bid> Bids { get; set; } = new();

        public int? WinningBidId { get; set; }
        public Bid? WinningBid { get; set; }
    }
}

