using System.Data;
using Dapper;
using Interprocess.Attending.Application.Abstractions.Data;
using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Clinics;

namespace Interprocess.Attending.Application.Clinics.SearchClinics;

internal class SearchClinicsQueryHandler : IQueryHandler<SearchClinicsQuery, IReadOnlyList<ClinicResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchClinicsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<ClinicResponse>>> Handle(SearchClinicsQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection sqlConnection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
                SELECT
                    c.id AS Id,
                    c.name AS Name
                FROM clinics c
                WHERE c.name LIKE @Name
                """;

        IEnumerable<ClinicResponse> clinics = await sqlConnection.QueryAsync<ClinicResponse>(
            sql,
            new
            {
                Name = $"%{request.Name}%"
            });

        if (!clinics.Any())
        {
            return Result.Failure<IReadOnlyList<ClinicResponse>>(ClinicErrors.NotFound);
        }

        return clinics.ToList();
    }
}