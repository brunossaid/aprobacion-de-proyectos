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
        private readonly DecisionService _decisionService;

        public ProjectApprovalStepController(DecisionService decisionService)
        {
            _decisionService = decisionService;
        }

        [HttpPatch("{id}/decision")]
        [ProducesResponseType(typeof(ProjectProposalDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [SwaggerOperation(Summary = "Actualizar estado de un paso de propuesta de proyecto")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] DecisionDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ErrorResponse
                {
                    Message = string.Join(" | ", errors)
                });
            }

            try
            {
                var proposalDto = await _decisionService.DecideAsync(id, dto);
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
