namespace AuctionApp.Data.DTO
{
    public class AuctionListItemDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public BidListItemDTO? WinningBid { get; set; } = null;
        public string UserName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<BidListItemDTO> Bids { get; set; } = new();

    }
}
