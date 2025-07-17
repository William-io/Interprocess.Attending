using Bogus;
using Dapper;
using Interprocess.Attending.Application.Abstractions.Data;
using System.Data;

namespace Interprocess.Attending.API.Extensions;

internal static class SeedData
{
    public static void Seeding(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker();

        List<object> clinics = new();
        for (int i = 0; i < 10; i++)
        {
            clinics.Add(new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName()
            });
        }

        const string sql = """
            INSERT INTO Clinics
            (Id, Name)
            VALUES(@Id, @Name);
            """;

        connection.Execute(sql, clinics);
    }
}
