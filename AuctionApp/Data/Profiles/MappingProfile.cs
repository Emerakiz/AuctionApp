using AuctionApp.Data.DTO;
using AuctionApp.Data.Models;
using AutoMapper;

namespace AuctionApp.Data.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, AuctionListItemDTO>()
                // If current price is null (no bids), use starting price
                .ForMember(dest => dest.CurrentPrice, opt => opt.MapFrom(src => src.CurrentPrice ?? src.StartingPrice));

            CreateMap<CreateAuctionDTO, Auction>();

            CreateMap<Bid, BidListItemDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name));

        }
    }
}
