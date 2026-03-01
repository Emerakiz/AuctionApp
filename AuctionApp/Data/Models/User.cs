namespace AuctionApp.Data.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;  

        public List<Auction> Auctions { get; set; } = new();
        public List<Bid> Bids { get; set; } = new();

    }
}
