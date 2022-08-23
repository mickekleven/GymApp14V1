using AutoMapper;
using GymApp14V1.Core.Models;
using GymApp14V1.Core.ViewModels;

namespace GymApp14V1.AutoMapper
{
    public class MapProfile : Profile
    {
        private readonly IMapper _mapper;

        public MapProfile()
        {
            CreateMap<GymClass, GymClassViewModel>()
                .ForMember(dest => dest.AttendingMembers, opt => opt.MapFrom(src => src.AttendingMembers));

            CreateMap<GymClass, GymClassViewModel>()
                .ForMember(dest => dest.AttendingMembers, opt => opt.MapFrom(src => src.AttendingMembers)).ReverseMap();

            CreateMap<ApplicationUser, MemberViewModel>()
                .ForMember(dest => dest.GymClasses, opt => opt.MapFrom(src => src.AttendedClasses));

            CreateMap<ApplicationUser, MemberViewModel>()
                .ForMember(dest => dest.GymClasses, opt => opt.MapFrom(src => src.AttendedClasses))
                .ReverseMap();


            //CreateMap<ApplicationUser, MemberViewModel>().ReverseMap();

        }

    }
}
