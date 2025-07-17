using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients;

namespace Interprocess.Attending.Application.Patients.UpdatePatient;

internal sealed class UpdatePatientCommandHandler : ICommandHandler<UpdatePatientCommand, Guid>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);

            if (patient is null)
            {
                return Result.Failure<Guid>(PatientErros.NotFound);
            }

            // Criar os novos value objects
            var sex = Enum.Parse<Sex>(request.Sex);
            var address = new Address(
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.District,
                request.Complement
            );

            // Atualizar o paciente
            patient.Update(request.Name, request.DateBirth, sex, address);

            _patientRepository.Update(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(patient.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(
                new Error("Patient.UpdateError", $"Erro ao atualizar paciente: {ex.Message}"));
        }
    }
}
