using Application.DTOs;
using Domain.Entities;
using AutoMapper;

namespace Application.Mappings
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
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleNavigation));

            CreateMap<ProjectProposal, ProjectProposalDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaNavigation))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeNavigation))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusNavigation))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateByNavigation));

            CreateMap<CreateProjectProposalDto, ProjectProposal>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ProjectType))
                .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy));

            CreateMap<ProjectApprovalStep, ProjectApprovalStepDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StepOrder, opt => opt.MapFrom(src => src.StepOrder))
                .ForMember(dest => dest.DecisionDate, opt => opt.MapFrom(src => src.DecisionDate))
                .ForMember(dest => dest.Observations, opt => opt.MapFrom(src => src.Observations))
                .ForMember(dest => dest.ApproverUser, opt => opt.MapFrom(src => src.ApproverUserNavigation))
                .ForMember(dest => dest.ApproverRole, opt => opt.MapFrom(src => src.ApproverRoleNavigation))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusNavigation));

            CreateMap<ProjectProposal, ProjectProposalListDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.AreaNavigation.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeNavigation.Name))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.EstimatedAmount))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.EstimatedDuration))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusNavigation.Name));
        }
    }
}
