using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class DecisionService
{
    private readonly ApprovalStepManager _stepManager;
    private readonly IProjectApprovalStepReader _stepReader;
    private readonly IMapper _mapper;

    public DecisionService(
        ApprovalStepManager stepManager,
        IProjectApprovalStepReader stepReader,
        IMapper mapper)
    {
        _stepManager = stepManager;
        _stepReader = stepReader;
        _mapper = mapper;
    }

    public async Task<ProjectProposalDto> DecideAsync(Guid projectId, DecisionDto dto)
    {
        var updatedProject = await _stepManager.DecideNextStepAsync(projectId, dto);

        var proposalDto = _mapper.Map<ProjectProposalDto>(updatedProject);

        var steps = await _stepReader.GetStepsByProjectIdAsync(projectId);
        proposalDto.Steps = _mapper.Map<List<ProjectApprovalStepDto>>(steps);

        return proposalDto;
    }
}
