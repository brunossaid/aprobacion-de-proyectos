using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectTypeController : ControllerBase
{
    private readonly IProjectTypeService _projectTypeService;

    public ProjectTypeController(IProjectTypeService projectTypeService)
    {
        _projectTypeService = projectTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectTypeDto>>> GetAll()
    {
        var types = await _projectTypeService.GetAllAsync();
        var typeDtos = types.Select(t => new ProjectTypeDto
        {
            Id = t.Id,
            Name = t.Name
        });

        return Ok(typeDtos);
    }
}
