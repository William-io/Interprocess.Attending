using Interprocess.Attending.Application.Attendances.GetAttendance;
using Interprocess.Attending.Application.Attendances.GetAttendancesByFilters;
using Interprocess.Attending.Application.Attendances.CreateAttendance;
using Interprocess.Attending.Application.Attendances.UpdateAttendance;
using Interprocess.Attending.Application.Attendances.InactivateAttendance;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interprocess.Attending.API.Controllers.Attendances;

[ApiController]
[Route("api/attendances")]
public class AttendanceController : ControllerBase
{
    private readonly ISender _sender;

    public AttendanceController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAttendances(CancellationToken cancellationToken)
    {
        var query = new GetAttendanceQuery();
        Result<IEnumerable<AttendanceResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("filters")]
    public async Task<IActionResult> GetAttendancesByFilters(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? patientId,
        [FromQuery] AttendanceStatus? status,
        CancellationToken cancellationToken)
    {
        var query = new GetAttendancesByFiltersQuery(startDate, endDate, patientId, status);
        Result<IEnumerable<AttendanceResponseFilter>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAttendance(
        [FromBody] CreateAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAttendanceCommand(
            request.ClinicId,
            request.PatientId,
            request.Description,
            request.StartedDate);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            CreatedAtAction(nameof(GetAttendances), new { id = result.Value }, result.Value) : 
            BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAttendance(
        Guid id,
        [FromBody] UpdateAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAttendanceCommand(
            id,
            request.Description,
            request.CreatedOnUtc);

        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> InactivateAttendance(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new InactivateAttendanceCommand(id);

        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
