using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Patients;
using MediatR;

namespace Interprocess.Attending.Application.Patients.RegisterPatient;

public class RegisterPatientCommandHandler : ICommandHandler<RegisterPatientCommand, Guid>
{
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterPatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {

        // Criar os value objects
        var firstName = new FirstName(request.FirstName);
        var lastName = new LastName(request.LastName);
        var cpf = new Document(request.Cpf);
        var sex = Enum.Parse<Sex>(request.Sex);
        var address = new Address(
            request.Street,
            request.City,
            request.State,
            request.ZipCode,
            request.District,
            request.Complement
        );

        // Criar o paciente com validação de CPF duplicado
        var patient = await Patient.CreateAsync(
            firstName,
            lastName,
            cpf,
            request.DateBirth,
            sex,
            PatientStatus.Active,
            address,
            _patientRepository);

        // Adicionar ao repositório
        _patientRepository.Add(patient);

        // Salvar no banco de dados
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(patient.Id);
    }
}
