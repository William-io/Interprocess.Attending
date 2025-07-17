namespace Interprocess.Attending.Domain.Patients;

public record Address(
    string Street,
    string City,
    string State,
    string ZipCode,
    string District,
    string Complement);
