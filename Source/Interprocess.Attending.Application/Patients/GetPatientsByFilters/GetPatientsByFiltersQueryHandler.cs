using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients;
using Interprocess.Attending.Application.Patients.GetPatient;

namespace Interprocess.Attending.Application.Patients.GetPatientsByFilters;

internal sealed class GetPatientsByFiltersQueryHandler : IQueryHandler<GetPatientsByFiltersQuery, IEnumerable<PatientResponse>>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientsByFiltersQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<IEnumerable<PatientResponse>>> Handle(GetPatientsByFiltersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var patients = await _patientRepository.GetByFiltersAsync(
                request.Name,
                request.Cpf,
                request.Status,
                cancellationToken);

            var response = patients.Select(patient => new PatientResponse
            {
                Id = patient.Id,
                Name = patient.Name,
                Cpf = patient.Cpf,
                DateBirth = patient.DateBirth,
                Sex = patient.Sex.ToString(),
                Status = patient.Status.ToString(),
                Street = patient.Address.Street,
                City = patient.Address.City,
                State = patient.Address.State,
                ZipCode = patient.Address.ZipCode,
                District = patient.Address.District,
                Complement = patient.Address.Complement
            });

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PatientResponse>>(
                new Error("Patient.GetByFiltersError", $"Erro ao buscar pacientes com filtros: {ex.Message}"));
        }
    }
}
