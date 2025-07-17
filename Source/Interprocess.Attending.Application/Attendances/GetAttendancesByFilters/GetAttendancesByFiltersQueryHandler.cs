using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances;
using Interprocess.Attending.Domain.Patients;
using Interprocess.Attending.Domain.Clinics;

namespace Interprocess.Attending.Application.Attendances.GetAttendancesByFilters;

internal sealed class GetAttendancesByFiltersQueryHandler : IQueryHandler<GetAttendancesByFiltersQuery, IEnumerable<AttendanceResponseFilter>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IClinicRepository _clinicRepository;

    public GetAttendancesByFiltersQueryHandler(
        IAttendanceRepository attendanceRepository,
        IPatientRepository patientRepository,
        IClinicRepository clinicRepository)
    {
        _attendanceRepository = attendanceRepository;
        _patientRepository = patientRepository;
        _clinicRepository = clinicRepository;
    }

    public async Task<Result<IEnumerable<AttendanceResponseFilter>>> Handle(GetAttendancesByFiltersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var attendances = await _attendanceRepository.GetByFiltersAsync(
                request.StartDate,
                request.EndDate,
                request.PatientId,
                request.Status,
                cancellationToken);

            var responses = new List<AttendanceResponseFilter>();

            foreach (var attendance in attendances)
            {
                var patient = await _patientRepository.GetByIdAsync(attendance.PatientId, cancellationToken);
                var clinic = await _clinicRepository.GetByIdAsync(attendance.ClinicId, cancellationToken);

                var response = new AttendanceResponseFilter
                {
                    Id = attendance.Id,
                    ClinicId = attendance.ClinicId,
                    PatientId = attendance.PatientId,
                    Description = attendance.Description,
                    CreatedOnUtc = attendance.CreatedOnUtc,
                    Status = attendance.Status.ToString(),
                    PatientName = patient != null ? $"{patient.Name}".Trim() : "N/A",
                    ClinicName = clinic?.Name ?? "N/A",
                    PatientCpf = patient?.Cpf ?? "N/A",
                };

                responses.Add(response);
            }

            // Ordenar por data decrescente
            var orderedResponses = responses.OrderByDescending(r => r.CreatedOnUtc).AsEnumerable();

            return Result.Success(orderedResponses);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<AttendanceResponseFilter>>(new Error("Exception", ex.Message));
        }
    }
}
