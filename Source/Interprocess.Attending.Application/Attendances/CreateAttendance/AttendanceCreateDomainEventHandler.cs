using Interprocess.Attending.Application.Abstractions.Registration;
using Interprocess.Attending.Domain.Attendances;
using Interprocess.Attending.Domain.Attendances.Events;
using Interprocess.Attending.Domain.Patients;
using MediatR;

namespace Interprocess.Attending.Application.Attendances.CreateAttendance;

public class AttendanceCreateDomainEventHandler : INotificationHandler<AttendanceCreatedDomainEvent>
{
    public AttendanceCreateDomainEventHandler(
        IAttendanceRepository attendanceRepository, 
        IPatientRepository patientRepository, 
        IRegistrationService registrationService)
    {
        _attendanceRepository = attendanceRepository;
        _patientRepository = patientRepository;
        _registrationService = registrationService;
    }

    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IRegistrationService _registrationService;
    
    public async Task Handle(AttendanceCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(notification.AttendanceId, cancellationToken);
        
        if (attendance is null)
            return;
        
        var patient = await _patientRepository.GetByIdAsync(attendance.PatientId, cancellationToken);
        
        if (patient is null)
            return;
        
        await _registrationService.SendRegisterConfirmationAsync(
            patient.Cpf, "Cadastro realizado com sucesso!");
    }
}