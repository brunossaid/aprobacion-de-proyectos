using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

public class ProposalDtoBuilderService
{
    private readonly IMapper _mapper;
    private readonly IProjectApprovalStepReader _stepReader;

    public ProposalDtoBuilderService(IMapper mapper, IProjectApprovalStepReader stepReader)
    {
        _mapper = mapper;
        _stepReader = stepReader;
    }

    public async Task<ProjectProposalDto> BuildAsync(ProjectProposal proposal)
    {
        var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);
        var steps = await _stepReader.GetStepsByProjectIdAsync(proposal.Id);
        proposalDto.Steps = _mapper.Map<List<ProjectApprovalStepDto>>(steps);
        return proposalDto;
    }
}
