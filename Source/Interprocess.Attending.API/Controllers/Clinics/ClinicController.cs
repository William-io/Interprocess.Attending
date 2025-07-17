using Interprocess.Attending.Application.Clinics.SearchClinics;
using Interprocess.Attending.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Interprocess.Attending.API.Controllers.Clinics;

[ApiController]
[Route("api/clinics")]
public class ClinicController : ControllerBase
{
    private readonly ISender _sender;
    
    public ClinicController(ISender sender)
    {
        _sender = sender;
    }
    
    // Define your endpoints here, for example:
    [HttpGet]
    public async Task<IActionResult> GetClinics(string name, CancellationToken cancellationToken)
    {
        var query = new SearchClinicsQuery(name);
        Result<IReadOnlyList<ClinicResponse>> result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
    
}