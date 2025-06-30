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
        private readonly ProposalDtoBuilderService _proposalDtoBuilderService;

        public ProjectProposalController(
            IProposalCreationService proposalCreationService,
            IProjectProposalReader proposalService,
            ProposalFilterService proposalFilterService,
            IProjectApprovalStepReader stepService,
            UpdateProposalService updateProposalService,
            ProposalDtoBuilderService proposalDtoBuilderService)
        {
            _proposalCreationService = proposalCreationService;
            _proposalService = proposalService;
            _proposalFilterService = proposalFilterService;
            _stepService = stepService;
            _updateProposalService = updateProposalService;
            _proposalDtoBuilderService = proposalDtoBuilderService;
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
                var proposalDto = await _proposalDtoBuilderService.BuildAsync(proposal);

                return CreatedAtAction(nameof(GetProposalById), new { id = proposal.Id }, proposalDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ErrorResponse { Message = ex.Message });
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

            var proposalDto = await _proposalDtoBuilderService.BuildAsync(proposal);

            return Ok(proposalDto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectProposalListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Filtrar propuestas de proyecto")]
        public async Task<ActionResult<List<ProjectProposalListDto>>> GetProposals([FromQuery] ProjectProposalFilterDto filters)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ErrorResponse { Message = string.Join(" | ", errors) });
            }

            var proposalDtos = await _proposalFilterService.GetFilteredDtoAsync(filters);

            return Ok(proposalDtos);
        }
        
        [HttpPatch("{id}")]
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

                var proposalDto = await _proposalDtoBuilderService.BuildAsync(updatedProposal);

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
