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
    
    [HttpGet]
    public async Task<IActionResult> GetClinics(CancellationToken cancellationToken)
    {
        var query = new SearchClinicsQuery();
        Result<IReadOnlyList<ClinicResponse>> result = await _sender.Send(query, cancellationToken);

        return Ok(result.Value);
    }
    
}