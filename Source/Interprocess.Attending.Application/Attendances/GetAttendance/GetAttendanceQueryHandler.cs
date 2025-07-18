using Dapper;
using Interprocess.Attending.Application.Abstractions.Data;
using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;

namespace Interprocess.Attending.Application.Attendances.GetAttendance;

internal sealed class GetAttendanceQueryHandler : IQueryHandler<GetAttendanceQuery, IEnumerable<AttendanceResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAttendanceQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IEnumerable<AttendanceResponse>>> Handle(GetAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                               a.Id AS Id,
                               a.ClinicId AS ClinicId,
                               a.PatientId AS PatientId,
                               a.Description AS Description,
                               a.CreatedOnUtc AS CreatedOnUtc,
                               a.Status AS Status,
                               p.Name AS PatientName,
                               c.Name AS ClinicName,
                               p.Cpf AS PatientCpf
                           FROM Attendances a
                           LEFT JOIN Patients p ON a.PatientId = p.Id
                           LEFT JOIN Clinics c ON a.ClinicId = c.Id
                           ORDER BY a.CreatedOnUtc DESC
                           """;

        var attendances = await connection.QueryAsync<AttendanceResponse>(sql);

        return Result.Success(attendances);
    }
}