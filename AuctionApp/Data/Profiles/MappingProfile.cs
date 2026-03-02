using AuctionApp.Data.DTO;
using AuctionApp.Data.Models;
using AutoMapper;

namespace AuctionApp.Data.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Auctions
            CreateMap<Auction, AuctionListItemDTO>()
                // If current price is null (no bids), use starting price
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.CurrentPrice ?? src.StartingPrice));

            CreateMap<CreateAuctionDTO, Auction>();

            // Bids
            CreateMap<Bid, BidListItemDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.BidDate, opt => opt.MapFrom(src => src.BidTime));
            CreateMap<PlaceBidDTO, Bid>();


            // User
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<LoginUserDTO, User>();
            CreateMap<RegisterUserDTO, User>();



        }
    }
}
