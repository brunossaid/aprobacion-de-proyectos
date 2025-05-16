using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;   
using Application.DTOs;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApproverRoleController : ControllerBase
{
    private readonly IApproverRoleService _approverRoleService;

    public ApproverRoleController(IApproverRoleService approverRoleService)
    {
        _approverRoleService = approverRoleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApproverRoleDto>>> GetAll()
    {
        var roles = await _approverRoleService.GetAllAsync();
        var rolesDtos = roles.Select(a => new ApproverRoleDto
    {
        Id = a.Id,
        Name = a.Name
    });

    return Ok(rolesDtos);
    }
}
