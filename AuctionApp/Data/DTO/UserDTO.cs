namespace AuctionApp.Data.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
