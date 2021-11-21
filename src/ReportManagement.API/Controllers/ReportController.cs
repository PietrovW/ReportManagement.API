using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportManagement.Application.Dtos;
using ReportManagement.Application.Queries;

namespace ReportManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> Get(string name)
        {
            return await _mediator.Send(new GetReportListQuery() { Name = name });
        }
    }
}
