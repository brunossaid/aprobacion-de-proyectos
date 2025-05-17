using Application.DTOs;
using Domain.Entities;
using AutoMapper;

namespace API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Area, AreaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<ApprovalStatus, ApprovalStatusDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<ProjectType, ProjectTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<ApproverRole, ApproverRoleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new ApproverRoleDto
                {
                    Id = src.Role, 
                    Name = src.RoleNavigation.Name 
                }));

            CreateMap<ProjectProposal, ProjectProposalDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaNavigation))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeNavigation))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusNavigation))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateByNavigation));

            CreateMap<CreateProjectProposalDto, ProjectProposal>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProjectType))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreatedBy));
        }
    }
}
