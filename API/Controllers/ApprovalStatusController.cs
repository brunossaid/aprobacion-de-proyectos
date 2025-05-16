using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;   
using Application.DTOs;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprovalStatusController : ControllerBase
{
    private readonly IApprovalStatusService _approvalStatusService;

    public ApprovalStatusController(IApprovalStatusService approvalStatusService)
    {
        _approvalStatusService = approvalStatusService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApprovalStatusDto>>> GetAll()
    {
        var statuses = await _approvalStatusService.GetAllAsync();
        var statusesDtos = statuses.Select(a => new ApprovalStatusDto
    {
        Id = a.Id,
        Name = a.Name
    });

    return Ok(statusesDtos);
    }
}
