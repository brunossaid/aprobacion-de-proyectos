using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Information")]
    public class ApprovalStatusController : ControllerBase
    {
        private readonly IApprovalStatusService _approvalStatusService;
        private readonly IMapper _mapper;

        public ApprovalStatusController(IApprovalStatusService approvalStatusService, IMapper mapper)
        {
            _approvalStatusService = approvalStatusService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalStatusDto>>> GetAll()
        {
            var statuses = await _approvalStatusService.GetAllAsync();

            var statusesDtos = _mapper.Map<IEnumerable<ApprovalStatusDto>>(statuses);

            return Ok(statusesDtos);
        }
    }
}
