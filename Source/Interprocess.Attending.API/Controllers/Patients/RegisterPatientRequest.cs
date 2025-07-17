namespace Interprocess.Attending.API.Controllers.Patients;

public sealed record RegisterPatientRequest(
    string Name,
    string Cpf,
    string DateBirth,
    string Sex,
    string Street,
    string City,
    string State,
    string ZipCode,
    string District,
    string Complement);
