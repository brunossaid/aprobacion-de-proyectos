using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;   
using Application.DTOs;
using AutoMapper;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Information")]
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;
    private readonly IMapper _mapper;

    public AreaController(IAreaService areaService, IMapper mapper)
    {
        _areaService = areaService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AreaDto>>> GetAll()
    {
        var areas = await _areaService.GetAllAsync();

        var areaDtos = _mapper.Map<IEnumerable<AreaDto>>(areas);

        return Ok(areaDtos);
    }
}
