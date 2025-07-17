using Interprocess.Attending.Application.Patients.GetPatient;
using Interprocess.Attending.Application.Patients.GetPatientsByFilters;
using Interprocess.Attending.Application.Patients.RegisterPatient;
using Interprocess.Attending.Application.Patients.UpdatePatient;
using Interprocess.Attending.Application.Patients.InactivatePatient;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interprocess.Attending.API.Controllers.Patients;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
    private readonly ISender _sender;

    public PatientController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients(CancellationToken cancellationToken)
    {
        var query = new GetPatientQuery();
        Result<IEnumerable<PatientResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("filters")]
    public async Task<IActionResult> GetPatientsByFilters(
        [FromQuery] string? name,
        [FromQuery] string? cpf,
        [FromQuery] PatientStatus? status,
        CancellationToken cancellationToken)
    {
        var query = new GetPatientsByFiltersQuery(name, cpf, status);
        Result<IEnumerable<PatientResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPatient(
        RegisterPatientRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }

        var command = new RegisterPatientCommand(
            request.Name,
            request.Cpf,
            request.DateBirth,
            request.Sex,
            request.Street,
            request.City,
            request.State,
            request.ZipCode,
            request.District,
            request.Complement);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? 
            CreatedAtAction(nameof(GetPatients), new { id = result.Value }, result.Value) : 
            BadRequest(result.Error);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePatient(
        Guid id,
        UpdatePatientRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null");
        }

        var command = new UpdatePatientCommand(
            id,
            request.Name,
            request.Cpf,
            request.DateBirth,
            request.Sex,
            request.Street,
            request.City,
            request.State,
            request.ZipCode,
            request.District,
            request.Complement);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> InactivatePatient(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new InactivatePatientCommand(id);

        Result result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}
