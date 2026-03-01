namespace AuctionApp.Core.Interfaces
{
    public interface IAdminService
    {
        Task<bool> IsUserAdminAsync(int userId);
        Task<bool> DeleteAuctionAsync(int auctionId, int userId);
        Task<bool> DeleteUserAsync(int userId, int adminUserId);
    }
}
