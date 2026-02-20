namespace AuctionApp.Data.Models
{
    public class Auction
    {
        public int AuctionId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
