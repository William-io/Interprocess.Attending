using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Patients;
using Interprocess.Attending.Application.Patients.GetPatient;

namespace Interprocess.Attending.Application.Patients.GetPatientsByFilters;

/*
Consulta com parametros opcionais para filtrar pacientes por nome, CPF e status.
*/
public sealed record GetPatientsByFiltersQuery(
    string? Name = null,
    string? Cpf = null,
    PatientStatus? Status = null
) : IQuery<IEnumerable<PatientResponse>>;
