using AutoMapper;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces; 

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/Project")]
    [Tags("Project")]
    public class ProjectProposalController : ControllerBase
    {
        private readonly IProposalCreationService _proposalCreationService;

        private readonly IProjectProposalService _proposalService;
        private readonly IMapper _mapper;

        public ProjectProposalController(
            IProposalCreationService proposalCreationService,
            IProjectProposalService proposalService,
            IMapper mapper)
        {
            _proposalCreationService = proposalCreationService;
            _proposalService = proposalService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectProposalDto>> CreateProposal([FromBody] CreateProjectProposalDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Datos inválidos");
            }

            var proposal = await _proposalCreationService.CreateProposalFromDtoAsync(dto);

            var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);

            return CreatedAtAction(nameof(GetProposalById), new { id = proposal.Id }, proposalDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectProposalDto>> GetProposalById(Guid id)
        {
            var proposal = await _proposalService.GetByIdAsync(id);

            if (proposal == null)
                return NotFound();

            var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);

            return Ok(proposalDto);
        }
    }
}
