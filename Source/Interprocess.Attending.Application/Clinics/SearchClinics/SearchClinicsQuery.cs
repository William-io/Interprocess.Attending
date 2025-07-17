using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Clinics.SearchClinics;

public record SearchClinicsQuery(string Name) : IQuery<IReadOnlyList<ClinicResponse>>;