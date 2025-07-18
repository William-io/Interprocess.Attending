using System.Data;
using Dapper;
using Interprocess.Attending.Application.Abstractions.Data;
using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;

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
                    Id AS Id,
                    Name AS Name
                FROM Clinics
                ORDER BY Name
                """;

        IEnumerable<ClinicResponse> clinics = await sqlConnection.QueryAsync<ClinicResponse>(sql);

        return clinics.ToList();
    }
}