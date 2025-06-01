using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/Role")]
    [Tags("Information")]
    public class ApproverRoleController : ControllerBase
    {
        private readonly IApproverRoleService _approverRoleService;
        private readonly IMapper _mapper;

        public ApproverRoleController(IApproverRoleService approverRoleService, IMapper mapper)
        {
            _approverRoleService = approverRoleService;
            _mapper = mapper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listado de roles de usuario")]
        public async Task<ActionResult<IEnumerable<ApproverRoleDto>>> GetAll()
        {
            var roles = await _approverRoleService.GetAllAsync();
            var rolesDtos = _mapper.Map<IEnumerable<ApproverRoleDto>>(roles);

            return Ok(rolesDtos);
        }
    }
}
