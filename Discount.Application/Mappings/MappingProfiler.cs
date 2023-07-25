using AutoMapper;
using Discount.Domain.Entities;
using Discount.Domain.Models;

namespace Discount.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDTO>().ReverseMap()
                .ForMember(dest => dest.Segment,
                            opt => opt.MapFrom(src => src.SegmentType));
            CreateMap<CustomerDTO, Customer>().ReverseMap()
                .ForMember(dest => dest.SegmentType,
                            opt => opt.MapFrom(src => src.Segment));
            CreateMap<Segment, SegmentDTO>().ReverseMap();
            CreateMap<SegmentDTO, Segment>().ReverseMap();
        }

    }
}
