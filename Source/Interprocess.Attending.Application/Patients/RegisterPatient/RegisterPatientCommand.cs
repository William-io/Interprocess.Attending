using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Patients.RegisterPatient;

public sealed record RegisterPatientCommand(
    string Name,
    string Cpf,
    string DateBirth,
    string Sex,
    string Street,
    string City,
    string State,
    string ZipCode,
    string District,
    string Complement) : ICommand<Guid>;