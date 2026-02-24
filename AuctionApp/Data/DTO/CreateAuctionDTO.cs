namespace AuctionApp.Data.DTO
{
    public class CreateAuctionDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StartingPrice { get; set; }

    }
}
