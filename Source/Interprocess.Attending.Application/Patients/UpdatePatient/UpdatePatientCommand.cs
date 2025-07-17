using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Patients.UpdatePatient;

public sealed record UpdatePatientCommand(
    Guid PatientId,
    string FirstName,
    string LastName,
    string Cpf,
    string DateBirth,
    string Sex,
    string Street,
    string City,
    string State,
    string ZipCode,
    string District,
    string Complement) : ICommand<Guid>;