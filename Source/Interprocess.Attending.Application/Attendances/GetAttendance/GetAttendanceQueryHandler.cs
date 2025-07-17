using Dapper;
using Interprocess.Attending.Application.Abstractions.Data;
using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Application.Abstractions.Patient;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances;

namespace Interprocess.Attending.Application.Attendances.GetAttendance;

internal sealed class GetAttendanceQueryHandler : IQueryHandler<GetAttendanceQuery, AttendanceResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetAttendanceQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<AttendanceResponse>> Handle(GetAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT
                               id AS Id,
                               clinic_id AS ClinicId,
                               patient_id AS PatientId,
                               description AS Description,
                               created_on_utc AS CreatedOnUtc
                               status AS Status,
                           FROM attendances
                           WHERE id = @AttendanceId
                           """;

        var attendance = await connection.QueryFirstOrDefaultAsync<AttendanceResponse>(
            sql,
            new
            {
                request.AttendanceId
            });

        return attendance;
    }
}