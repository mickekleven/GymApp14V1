using AutoMapper;
using GymApp14V1.Models;
using GymApp14V1.Models.ViewModels;

namespace GymApp14V1.AutoMapper
{
    public class MapProfile : Profile
    {
        private readonly IMapper _mapper;

        public MapProfile()
        {
            CreateMap<GymClass, GymClassViewModel>();
            CreateMap<GymClass, GymClassViewModel>().ReverseMap();

            CreateMap<ApplicationUser, MemberViewModel>();
            CreateMap<ApplicationUser, MemberViewModel>().ReverseMap();

        }

    }
}
