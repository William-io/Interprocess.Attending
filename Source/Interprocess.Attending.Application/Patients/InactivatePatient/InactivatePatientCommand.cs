using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Patients.InactivatePatient;

public sealed record InactivatePatientCommand(Guid PatientId) : ICommand;
