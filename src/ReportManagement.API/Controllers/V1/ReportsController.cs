using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportManagement.Application.Common.V1;
using ReportManagement.Application.Dtos.V1;
using ReportManagement.Application.Queries.V1;
using ReportManagement.Application.Request.V1;

namespace ReportManagement.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IMediator _mediator;

        public ReportsController(
            ILogger<ReportsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}.{format?}"), FormatFilter]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(await _mediator.Send(new GetReportQuery() { Id = id }));
        }

        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<CreateReportRequest> PostAsync(CreateReportRequest request)
        {
            var command = new CreateReportCommand() { Name = request.Name };
            CreateReportRequest result = await _mediator.Send(command);
            return result;
        }
    }
}
