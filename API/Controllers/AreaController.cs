using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;   
using Application.DTOs;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;

    public AreaController(IAreaService areaService)
    {
        _areaService = areaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetAll()
    {
        var areas = await _areaService.GetAllAsync();
        var areaDtos = areas.Select(a => new AreaDto
    {
        Id = a.Id,
        Name = a.Name
    });

    return Ok(areaDtos);
    }
}
