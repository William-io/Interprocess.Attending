using Interprocess.Attending.Application.Abstractions.Clock;
using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances;
using Interprocess.Attending.Domain.Clinics;
using Interprocess.Attending.Domain.Patients;

namespace Interprocess.Attending.Application.Attendances.CreateAttendance;

internal sealed class CreateAttendanceCommandHandler : ICommandHandler<CreateAttendanceCommand, Guid>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IClinicRepository _clinicRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateAttendanceCommandHandler(
        IAttendanceRepository attendanceRepository,
        IPatientRepository patientRepository,
        IClinicRepository clinicRepository,
        IUnitOfWork unitOfWork, 
        IDateTimeProvider dateTimeProvider)
    {
        _attendanceRepository = attendanceRepository;
        _patientRepository = patientRepository;
        _clinicRepository = clinicRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<Guid>> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
        
        if (patient is null) 
            return Result.Failure<Guid>(PatientErros.NotFound);
        
        var clinic = await _clinicRepository.GetByIdAsync(request.ClinicId, cancellationToken);
        
        if (clinic is null) 
            return Result.Failure<Guid>(ClinicErrors.NotFound);

        try
        {
            var attendance = Attendance.MakeAnAttendanceRecord(
                clinic,
                patient.Id,
                request.Description,
                _dateTimeProvider.UtcNow);
            
            _attendanceRepository.Add(attendance);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(attendance.Id);

        }
        catch (Exception)
        {
            return Result.Failure<Guid>(AttendanceErrors.CreationFailed);
        }
    }
}