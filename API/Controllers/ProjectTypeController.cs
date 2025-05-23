using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Information")]
    public class ProjectTypeController : ControllerBase
    {
        private readonly IProjectTypeService _projectTypeService;
        private readonly IMapper _mapper;

        public ProjectTypeController(IProjectTypeService projectTypeService, IMapper mapper)
        {
            _projectTypeService = projectTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de tipos de proyecto")]
        public async Task<ActionResult<IEnumerable<ProjectTypeDto>>> GetAll()
        {
            var types = await _projectTypeService.GetAllAsync();
            var typeDtos = _mapper.Map<IEnumerable<ProjectTypeDto>>(types);

            return Ok(typeDtos);
        }
    }
}
