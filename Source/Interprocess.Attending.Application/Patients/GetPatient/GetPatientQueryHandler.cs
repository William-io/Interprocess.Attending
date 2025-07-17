using MediatR;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients;
using Interprocess.Attending.Application.Abstractions.MessageCommunication;

namespace Interprocess.Attending.Application.Patients.GetPatient;

internal sealed class GetPatientQueryHandler : IQueryHandler<GetPatientQuery, IEnumerable<PatientResponse>>
{
    private readonly IPatientRepository _patientRepository;

    public GetPatientQueryHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<IEnumerable<PatientResponse>>> Handle(GetPatientQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var patients = await _patientRepository.GetAllAsync(cancellationToken);

            var responses = patients.Select(patient => new PatientResponse
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

            return Result.Success(responses);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PatientResponse>>(
                new Error("Patient.GetError", $"Erro ao buscar pacientes: {ex.Message}"));
        }
    }
}
