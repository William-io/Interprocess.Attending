using Interprocess.Attending.Application.Abstractions.MessageCommunication;
using Interprocess.Attending.Domain.Abstractions;
using Interprocess.Attending.Domain.Attendances;

namespace Interprocess.Attending.Application.Attendances.InactivateAttendance;

internal sealed class InactivateAttendanceCommandHandler : ICommandHandler<InactivateAttendanceCommand>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public InactivateAttendanceCommandHandler(
        IAttendanceRepository attendanceRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRepository = attendanceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(InactivateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var attendance = await _attendanceRepository.GetByIdAsync(request.AttendanceId, cancellationToken);

        if (attendance is null)
        {
            return Result.Failure(AttendanceErrors.NotFound);
        }

        var inactivateResult = attendance.Inactivate();

        if (inactivateResult.IsFailure)
        {
            return inactivateResult;
        }

        _attendanceRepository.Update(attendance);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
