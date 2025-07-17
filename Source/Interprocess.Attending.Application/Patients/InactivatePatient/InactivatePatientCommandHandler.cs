using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients;

namespace Interprocess.Attending.Application.Patients.InactivatePatient;

internal sealed class InactivatePatientCommandHandler : ICommandHandler<InactivatePatientCommand>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InactivatePatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(InactivatePatientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);

            if (patient is null)
                return Result.Failure(PatientErros.NotFound);

            if (patient.Status == PatientStatus.Inactive)          
                return Result.Failure(PatientErros.AlreadyInactive);
            
            // Inativar o paciente
            patient.Inactivate();

            _patientRepository.Update(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(
                new Error("Patient.InactivateError", $"Erro ao inativar paciente: {ex.Message}"));
        }
    }
}
