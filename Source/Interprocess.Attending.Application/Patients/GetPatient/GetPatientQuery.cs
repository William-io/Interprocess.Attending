using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Patients.GetPatient;

public sealed record GetPatientQuery() : IQuery<IEnumerable<PatientResponse>>;