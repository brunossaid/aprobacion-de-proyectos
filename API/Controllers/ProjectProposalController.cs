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
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ProjectProposalDto>> CreateProposal([FromBody] CreateProjectProposalDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ErrorResponse { Message = "Datos del proyecto invalidos" });
            }
            try
            {
                var proposal = await _proposalCreationService.CreateProposalFromDtoAsync(dto);
                var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);

                return CreatedAtAction(nameof(GetProposalById), new { id = proposal.Id }, proposalDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErrorResponse { Message = ex.Message }); // <-- devuelve 409 con mensaje
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectProposalDto>> GetProposalById(Guid id)
        {
            var proposal = await _proposalService.GetByIdAsync(id);

            if (proposal == null)
                return NotFound();

            var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);

            return Ok(proposalDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectProposalDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<ProjectProposalDto>>> GetProposals([FromQuery] ProjectProposalFilterDto filters)
        {
            if (filters.Status.HasValue && (filters.Status < 1 || filters.Status > 4))
            {
                return BadRequest(new ErrorResponse { Message = "El estado ingresado no es valido" });
            }

            if (filters.CreatedBy.HasValue && filters.CreatedBy <= 0)
            {
                return BadRequest(new ErrorResponse { Message = "El ID del usuario creador debe ser mayor a 0" });
            }

            if (filters.ApproverUserId.HasValue && filters.ApproverUserId <= 0)
            {
                return BadRequest(new ErrorResponse { Message = "El ID del aprobador debe ser mayor a 0" });
            }

            var proposals = await _proposalService.GetFilteredAsync(filters);
            var proposalDtos = _mapper.Map<List<ProjectProposalDto>>(proposals);

            return Ok(proposalDtos);
        }
    }
}
