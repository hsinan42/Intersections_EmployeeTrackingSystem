using AutoMapper;
using BusinessLayer.DTOs;
using EntityLayer.Concrete;
using Intersections_EmployeeTrackingSystem.Models;

namespace Intersections_EmployeeTrackingSystem.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IntersectionViewModel, Intersection>()
                .ForMember(dest => dest.IntersectionImages, opt => opt.Ignore())
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(dest => dest.Substructure, opt => opt.MapFrom(src => src.Subtructure))
                .ForMember(dest => dest.Reports, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<LocationViewModel, Location>().ReverseMap();
            CreateMap<SubtructersViewModel, Substructure>().ReverseMap();
            CreateMap<Report, ReportsViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<RegisterViewModel, User>().ReverseMap();

            CreateMap<User, EmployeeViewModel>()
                .ForMember(dest => dest.EmployeIntersections, opt => opt.MapFrom(src => src.Intersections))
                .ForMember(dest => dest.EmployeeReports, opt => opt.MapFrom(src => src.Reports))
                .ReverseMap()
                .ForMember(dest => dest.Intersections, opt => opt.MapFrom(src => src.EmployeIntersections))
                .ForMember(dest => dest.Reports, opt => opt.MapFrom(src => src.EmployeeReports));

            CreateMap<EmployeeIntersectionsVM, Intersection>()
                .ReverseMap();
            CreateMap<Report, EmployeeReportsVM>()
                .ForMember(dest => dest.IntersectionTitle,
                           opt => opt.MapFrom(src => src.intersection.Title))
                .ForMember(dest => dest.KkcId,
                           opt => opt.MapFrom(src => src.intersection.KkcID))
                .ForMember(dest => dest.IntersectionStatus,
                           opt => opt.MapFrom(src => src.intersection.IntersectionStatus));


            CreateMap<Intersection, DetailsVM>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.Subtructure, o => o.MapFrom(s => s.Substructure))
                .ForMember(d => d.Locations, o => o.MapFrom(s => s.Locations))
                .ForMember(d => d.Reports, o => o.MapFrom(s => s.Reports))
                .ReverseMap()
                .ForMember(s => s.User, o => o.Ignore())
                .ForMember(s => s.Substructure, o => o.Ignore())
                .ForMember(s => s.Locations, o => o.Ignore())
                .ForMember(s => s.Reports, o => o.Ignore());

            CreateMap<Report, DetailsRepVM>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
                .ReverseMap()
                .ForMember(s => s.User, o => o.Ignore())
                .ForMember(s => s.intersection, o => o.Ignore());

            CreateMap<Location, DetailsLocVM>();

            CreateMap<Substructure, DetailsSubVM>();

            CreateMap<Intersection, IntersectionUpdateDto>()
                .ForMember(d => d.Substructure, opt => opt.MapFrom(s => s.Substructure))
                .ForMember(d => d.Locations, opt => opt.MapFrom(s => s.Locations));

            CreateMap<Substructure, SubstructureDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();

            CreateMap<IntersectionUpdateDto, Intersection>()
                .ForMember(d => d.Locations, opt => opt.Ignore())
                .ForMember(d => d.IntersectionImages, opt => opt.Ignore())
                .ForMember(d => d.Substructure, opt => opt.Ignore());

            CreateMap<LocationViewModel, LocationDto>().ReverseMap();
            CreateMap<SubtructersViewModel, SubstructureDto>().ReverseMap();

            CreateMap<IntersectionViewModel, IntersectionUpdateDto>()
                .ForMember(d => d.Substructure, opt => opt.MapFrom(s => s.Subtructure))
                .ForMember(d => d.Locations, opt => opt.MapFrom(s => s.Locations))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}

