namespace Interprocess.Attending.Application.Patients.GetPatient;

public sealed class PatientResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Cpf { get; init; } = string.Empty;
    public string DateBirth { get; init; } = string.Empty;
    public string Sex { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public string Complement { get; init; } = string.Empty;
}
