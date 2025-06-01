using AutoMapper;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/Project")]
    [Tags("Project")]
    public class ProjectProposalController : ControllerBase
    {
        private readonly IProposalCreationService _proposalCreationService;
        private readonly IProjectProposalReader _proposalService;
        private readonly ProposalFilterService _proposalFilterService;
        private readonly IProjectApprovalStepReader _stepService;
        private readonly UpdateProposalService _updateProposalService;
        private readonly IMapper _mapper;

        public ProjectProposalController(
            IProposalCreationService proposalCreationService,
            IProjectProposalReader proposalService,
            ProposalFilterService proposalFilterService,
            IProjectApprovalStepReader stepService,
            UpdateProposalService updateProposalService,
            IMapper mapper)
        {
            _proposalCreationService = proposalCreationService;
            _proposalService = proposalService;
            _proposalFilterService = proposalFilterService;
            _stepService = stepService;
            _updateProposalService = updateProposalService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Crear propuesta de proyecto")]
        public async Task<ActionResult<ProjectProposalDto>> CreateProposal([FromBody] CreateProjectProposalDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ErrorResponse { Message = "Datos del proyecto invalidos" });
            }

            try
            {
                var proposal = await _proposalCreationService.CreateProposalAsync(dto);
                var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);

                var steps = await _stepService.GetStepsByProjectIdAsync(proposal.Id);
                proposalDto.ApprovalSteps = _mapper.Map<List<ProjectApprovalStepDto>>(steps);

                return CreatedAtAction(nameof(GetProposalById), new { id = proposal.Id }, proposalDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErrorResponse { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Buscar propuesta de proyecto por ID")]
        public async Task<ActionResult<ProjectProposalDto>> GetProposalById(Guid id)
        {
            var proposal = await _proposalService.GetByIdAsync(id);

            if (proposal == null)
                return NotFound(new ErrorResponse { Message = "No se encontro la propuesta solicitada" });

            var steps = await _stepService.GetStepsByProjectIdAsync(id);
            var proposalDto = _mapper.Map<ProjectProposalDto>(proposal);
            proposalDto.ApprovalSteps = _mapper.Map<List<ProjectApprovalStepDto>>(steps);
            return Ok(proposalDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectProposalListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Filtrar propuestas de proyecto")]
        public async Task<ActionResult<List<ProjectProposalListDto>>> GetProposals([FromQuery] ProjectProposalFilterDto filters)
        {
            if (filters.Status.HasValue && (filters.Status < 1 || filters.Status > 4))
            {
                return BadRequest(new ErrorResponse { Message = "El estado ingresado no es valido" });
            }

            if (filters.CreateBy.HasValue && filters.CreateBy <= 0)
            {
                return BadRequest(new ErrorResponse { Message = "El ID del creador debe ser mayor a 0" });
            }

            if (filters.ApproverUserId.HasValue && filters.ApproverUserId <= 0)
            {
                return BadRequest(new ErrorResponse { Message = "El ID del aprobador debe ser mayor a 0" });
            }

            var proposals = await _proposalFilterService.GetFilteredAsync(filters);
            var proposalDtos = _mapper.Map<List<ProjectProposalListDto>>(proposals);

            return Ok(proposalDtos);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Actualizar propuesta de proyecto")]
        public async Task<IActionResult> UpdateProposal(Guid id, [FromBody] UpdateProjectProposalDto dto)
        {
            try
            {
                var updatedProposal = await _updateProposalService.UpdateProposalAsync(id, dto);

                var steps = await _stepService.GetStepsByProjectIdAsync(updatedProposal.Id);
                var proposalDto = _mapper.Map<ProjectProposalDto>(updatedProposal);
                proposalDto.ApprovalSteps = _mapper.Map<List<ProjectApprovalStepDto>>(steps);

                return Ok(proposalDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ErrorResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
            }
        }
    }
}
