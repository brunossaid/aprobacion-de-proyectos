using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;
using Application.Interfaces;
using Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/Project")]
    [Tags("Project")]
    public class ProjectApprovalStepController : ControllerBase
    {
        private readonly ApprovalStepManager _approvalStepManager;
        private readonly IMapper _mapper;
        private readonly IProjectApprovalStepReader _stepService; 

        public ProjectApprovalStepController(ApprovalStepManager approvalStepManager, IProjectApprovalStepReader stepService,  IMapper mapper)
        {
            _approvalStepManager = approvalStepManager;
            _stepService = stepService;
            _mapper = mapper;
        }

        [HttpPut("{id}/decision")]
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Actualizar estado de un paso de propuesta de proyecto")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] DecisionDto dto)
        {
            try
            {
                var updatedProject = await _approvalStepManager.DecideNextStepAsync(id, dto);
                Console.WriteLine("updatedProject", updatedProject);

                var proposalDto = _mapper.Map<ProjectProposalDto>(updatedProject);

                var steps = await _stepService.GetStepsByProjectIdAsync(updatedProject.Id);
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
